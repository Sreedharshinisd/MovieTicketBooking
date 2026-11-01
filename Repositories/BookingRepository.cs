using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking, List<int> seatIds)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var show = await _context.Shows
                    .Include(s => s.ShowSeats)
                    .FirstOrDefaultAsync(s => s.ShowId == booking.ShowId);

                if (show == null)
                    throw new ArgumentException("Show not found.");

                if (seatIds == null || seatIds.Count == 0)
                    throw new ArgumentException("Select at least one seat.");

                if (seatIds.Distinct().Count() != seatIds.Count)
                    throw new ArgumentException("Duplicate seats are not allowed.");

                var selectedSeats = show.ShowSeats
                    .Where(s => seatIds.Contains(s.SeatId))
                    .ToList();

                if (selectedSeats.Count != seatIds.Count)
                    throw new ArgumentException("One or more seats do not belong to this show.");

                if (selectedSeats.Any(s => s.Status != "Available"))
                    throw new ArgumentException("One or more selected seats are already booked.");

                foreach (var seat in selectedSeats)
                    seat.Status = "Booked";

                booking.TotalAmount = show.TicketPrice * seatIds.Count;
                booking.BookingDate = DateTime.UtcNow;
                booking.Status = "Confirmed";

                booking.BookingSeats = seatIds.Select(seatId => new BookingSeat
                {
                    SeatId = seatId
                }).ToList();

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.BookingSeats)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.BookingSeats)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<List<Booking>> GetBookingsByShowIdAsync(int showId)
        {
            return await _context.Bookings
                .Include(b => b.BookingSeats)
                .Where(b => b.ShowId == showId)
                .ToListAsync();
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.BookingSeats)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking == null)
                    return false;

                if (booking.Status == "Cancelled")
                    throw new ArgumentException("Booking is already cancelled.");

                var seatIds = booking.BookingSeats.Select(bs => bs.SeatId).ToList();

                var showSeats = await _context.ShowSeats
                    .Where(ss => ss.ShowId == booking.ShowId && seatIds.Contains(ss.SeatId))
                    .ToListAsync();

                foreach (var seat in showSeats)
                    seat.Status = "Available";

                booking.Status = "Cancelled";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

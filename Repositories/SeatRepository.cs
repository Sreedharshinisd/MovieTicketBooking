using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Seat> AddSeatAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();
            return seat;
        }

        public async Task<List<Seat>> AddSeatsAsync(List<Seat> seats)
        {
            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
            return seats;
        }

        public async Task<List<Seat>> GetSeatsByScreenIdAsync(int screenId)
        {
            return await _context.Seats
                .Where(s => s.ScreenId == screenId)
                .ToListAsync();
        }

        public async Task<Seat?> GetSeatByIdAsync(int seatId)
        {
            return await _context.Seats
                .FirstOrDefaultAsync(s => s.SeatId == seatId);
        }

        public async Task UpdateSeatAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSeatAsync(Seat seat)
        {
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
        }
    }
}

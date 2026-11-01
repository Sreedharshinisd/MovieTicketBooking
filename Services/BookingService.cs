using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CustomerName))
                throw new ArgumentException("Customer name is required.");

            if (string.IsNullOrWhiteSpace(dto.CustomerEmail) || !dto.CustomerEmail.Contains('@'))
                throw new ArgumentException("Enter a valid email address.");

            if (string.IsNullOrWhiteSpace(dto.CustomerPhone))
                throw new ArgumentException("Customer phone is required.");

            if (dto.ShowId <= 0)
                throw new ArgumentException("Enter a valid Show ID.");

            if (dto.SeatIds == null || dto.SeatIds.Count == 0)
                throw new ArgumentException("Select at least one seat.");

            var booking = new Booking
            {
                ShowId = dto.ShowId,
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                CustomerPhone = dto.CustomerPhone
            };

            var result = await _bookingRepository.CreateBookingAsync(booking, dto.SeatIds);
            return MapToResponse(result);
        }

        public async Task<List<BookingResponseDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            return bookings.Select(MapToResponse).ToList();
        }

        public async Task<BookingResponseDto?> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            return booking == null ? null : MapToResponse(booking);
        }

        public async Task<List<BookingResponseDto>> GetBookingsByShowIdAsync(int showId)
        {
            var bookings = await _bookingRepository.GetBookingsByShowIdAsync(showId);
            return bookings.Select(MapToResponse).ToList();
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            return await _bookingRepository.CancelBookingAsync(bookingId);
        }

        private static BookingResponseDto MapToResponse(Booking booking)
        {
            return new BookingResponseDto
            {
                BookingId = booking.BookingId,
                ShowId = booking.ShowId,
                CustomerName = booking.CustomerName,
                CustomerEmail = booking.CustomerEmail,
                CustomerPhone = booking.CustomerPhone,
                BookingDate = booking.BookingDate,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,
                SeatIds = booking.BookingSeats.Select(bs => bs.SeatId).ToList()
            };
        }
    }
}

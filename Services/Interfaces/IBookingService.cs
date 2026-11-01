using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto bookingDto);
        Task<List<BookingResponseDto>> GetAllBookingsAsync();
        Task<BookingResponseDto?> GetBookingByIdAsync(int bookingId);
        Task<List<BookingResponseDto>> GetBookingsByShowIdAsync(int showId);
        Task<bool> CancelBookingAsync(int bookingId);
    }
}

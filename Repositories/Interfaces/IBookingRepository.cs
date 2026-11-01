using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBookingAsync(Booking booking, List<int> seatIds);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<List<Booking>> GetBookingsByShowIdAsync(int showId);
        Task<bool> CancelBookingAsync(int bookingId);
    }
}

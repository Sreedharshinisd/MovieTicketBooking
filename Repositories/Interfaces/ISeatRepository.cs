using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<Seat> AddSeatAsync(Seat seat);
        Task<List<Seat>> AddSeatsAsync(List<Seat> seats);
        Task<List<Seat>> GetSeatsByScreenIdAsync(int screenId);
        Task<Seat?> GetSeatByIdAsync(int seatId);
        Task UpdateSeatAsync(Seat seat);
        Task DeleteSeatAsync(Seat seat);
    }
}

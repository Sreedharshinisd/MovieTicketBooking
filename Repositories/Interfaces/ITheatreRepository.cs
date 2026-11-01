using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ITheatreRepository
    {
        Task<Theatre> AddTheatreAsync(Theatre theatre);

        Task<List<Theatre>> GetAllTheatresAsync();

        Task<Theatre?> GetTheatreByIdAsync(int theatreId);

        Task UpdateTheatreAsync(Theatre theatre);

        Task DeleteTheatreAsync(Theatre theatre);
    }
}
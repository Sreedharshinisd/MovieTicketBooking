using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IScreenRepository
    {
        Task<Screen> AddScreenAsync(Screen screen);

        Task<List<Screen>> GetAllScreensAsync();

        Task<Screen?> GetScreenByIdAsync(int screenId);

        Task<List<Screen>> GetScreensByTheatreIdAsync(int theatreId);

        Task UpdateScreenAsync(Screen screen);

        Task DeleteScreenAsync(Screen screen);
    }
}
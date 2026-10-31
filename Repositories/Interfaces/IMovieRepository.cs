
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie> AddMovieAsync(Movie movie);

        Task<List<Movie>> GetAllMoviesAsync();

        Task<Movie?> GetMovieByIdAsync(int movieId);

        Task UpdateMovieAsync(Movie movie);

        Task DeleteMovieAsync(Movie movie);

        Task<List<Movie>> SearchMoviesByTitleAsync(string title);
    }
}


using MovieTicketBooking.DTOs.Movies;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IMovieService
    {
        Task<MovieResponseDto> AddMovieAsync(CreateMovieDto movieDto);

        Task<List<MovieResponseDto>> GetAllMoviesAsync();

        Task<MovieResponseDto?> GetMovieByIdAsync(int movieId);

        Task<bool> UpdateMovieAsync(int movieId, UpdateMovieDto movieDto);

        Task<bool> DeleteMovieAsync(int movieId);

        Task<List<MovieResponseDto>> SearchMoviesByTitleAsync(string title);
    }
}

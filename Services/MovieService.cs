
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<MovieResponseDto> AddMovieAsync(CreateMovieDto movieDto)
        {
            ValidateMovie(
                movieDto.Title,
                movieDto.DurationMinutes,
                movieDto.Language,
                movieDto.Genre);

            var movie = new Movie
            {
                Title = movieDto.Title.Trim(),
                Description = movieDto.Description,
                DurationMinutes = movieDto.DurationMinutes,
                ReleaseDate = movieDto.ReleaseDate,
                Language = movieDto.Language.Trim(),
                Genre = movieDto.Genre.Trim()
            };

            var addedMovie = await _movieRepository.AddMovieAsync(movie);

            return MapToResponseDto(addedMovie);
        }

        public async Task<List<MovieResponseDto>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();

            return movies.Select(MapToResponseDto).ToList();
        }

        public async Task<MovieResponseDto?> GetMovieByIdAsync(int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            return movie == null ? null : MapToResponseDto(movie);
        }

        public async Task<bool> UpdateMovieAsync(int movieId, UpdateMovieDto movieDto)
        {
            ValidateMovie(
                movieDto.Title,
                movieDto.DurationMinutes,
                movieDto.Language,
                movieDto.Genre);

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if (movie == null)
                return false;

            movie.Title = movieDto.Title.Trim();
            movie.Description = movieDto.Description;
            movie.DurationMinutes = movieDto.DurationMinutes;
            movie.ReleaseDate = movieDto.ReleaseDate;
            movie.Language = movieDto.Language.Trim();
            movie.Genre = movieDto.Genre.Trim();

            await _movieRepository.UpdateMovieAsync(movie);

            return true;
        }

        public async Task<bool> DeleteMovieAsync(int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if (movie == null)
                return false;

            await _movieRepository.DeleteMovieAsync(movie);

            return true;
        }

        public async Task<List<MovieResponseDto>> SearchMoviesByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Search title cannot be empty.");

            var movies = await _movieRepository.SearchMoviesByTitleAsync(title.Trim());

            return movies.Select(MapToResponseDto).ToList();
        }

        private static void ValidateMovie(
            string title,
            int duration,
            string language,
            string genre)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Movie title is required.");

            if (duration <= 0)
                throw new ArgumentException("Duration must be greater than zero.");

            if (string.IsNullOrWhiteSpace(language))
                throw new ArgumentException("Language is required.");

            if (string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Genre is required.");
        }

        private static MovieResponseDto MapToResponseDto(Movie movie)
        {
            return new MovieResponseDto
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                Genre = movie.Genre
            };
        }
    }
}

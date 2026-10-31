
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // POST: api/movies
        [HttpPost]
        public async Task<IActionResult> AddMovie(CreateMovieDto movieDto)
        {
            var movie = await _movieService.AddMovieAsync(movieDto);

            return CreatedAtAction(
                nameof(GetMovieById),
                new { id = movie.MovieId },
                movie);
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();

            return Ok(movies);
        }

        // GET: api/movies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);

            if (movie == null)
                return NotFound(new { message = "Movie not found." });

            return Ok(movie);
        }

        // PUT: api/movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(
            int id,
            UpdateMovieDto movieDto)
        {
            var updated = await _movieService.UpdateMovieAsync(id, movieDto);

            if (!updated)
                return NotFound(new { message = "Movie not found." });

            return Ok(new { message = "Movie updated successfully." });
        }

        // DELETE: api/movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var deleted = await _movieService.DeleteMovieAsync(id);

            if (!deleted)
                return NotFound(new { message = "Movie not found." });

            return Ok(new { message = "Movie deleted successfully." });
        }

        // GET: api/movies/search?title=leo
        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies([FromQuery] string title)
        {
            var movies = await _movieService.SearchMoviesByTitleAsync(title);

            return Ok(movies);
        }
    }
}

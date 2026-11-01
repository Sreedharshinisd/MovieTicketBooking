using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShow(CreateShowDto showDto)
        {
            try
            {
                var result = await _showService.CreateShowAsync(showDto);
                return CreatedAtAction(nameof(GetShowById),
                    new { id = result.ShowId }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShows()
        {
            var shows = await _showService.GetAllShowsAsync();
            return Ok(shows);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var show = await _showService.GetShowByIdAsync(id);

            if (show == null)
                return NotFound(new { message = "Show not found." });

            return Ok(show);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetShowsByMovie(int movieId)
        {
            var shows = await _showService.GetShowsByMovieIdAsync(movieId);
            return Ok(shows);
        }

        [HttpGet("screen/{screenId}")]
        public async Task<IActionResult> GetShowsByScreen(int screenId)
        {
            var shows = await _showService.GetShowsByScreenIdAsync(screenId);
            return Ok(shows);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShow(int id, UpdateShowDto showDto)
        {
            try
            {
                var updated = await _showService.UpdateShowAsync(id, showDto);

                if (!updated)
                    return NotFound(new { message = "Show not found." });

                return Ok(new { message = "Show updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var deleted = await _showService.DeleteShowAsync(id);

            if (!deleted)
                return NotFound(new { message = "Show not found." });

            return Ok(new { message = "Show deleted successfully." });
        }

        [HttpGet("{showId}/seats")]
        public async Task<IActionResult> GetShowSeats(int showId)
        {
            try
            {
                return Ok(await _showService.GetShowSeatsAsync(showId));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{showId}/available")]
        public async Task<IActionResult> GetAvailableSeats(int showId)
        {
            try
            {
                return Ok(await _showService.GetAvailableSeatsAsync(showId));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{showId}/booked")]
        public async Task<IActionResult> GetBookedSeats(int showId)
        {
            try
            {
                return Ok(await _showService.GetBookedSeatsAsync(showId));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
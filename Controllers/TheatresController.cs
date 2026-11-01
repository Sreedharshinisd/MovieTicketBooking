
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreService _theatreService;

        public TheatresController(ITheatreService theatreService)
        {
            _theatreService = theatreService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTheatre(CreateTheatreDto theatreDto)
        {
            var result = await _theatreService.AddTheatreAsync(theatreDto);

            return CreatedAtAction(
                nameof(GetTheatreById),
                new { id = result.TheatreId },
                result
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTheatres()
        {
            var theatres = await _theatreService.GetAllTheatresAsync();

            return Ok(theatres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTheatreById(int id)
        {
            var theatre = await _theatreService.GetTheatreByIdAsync(id);

            if (theatre == null)
                return NotFound("Theatre not found.");

            return Ok(theatre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheatre(
            int id, UpdateTheatreDto theatreDto)
        {
            var updated = await _theatreService.UpdateTheatreAsync(id, theatreDto);

            if (!updated)
                return NotFound("Theatre not found.");

            return Ok("Theatre updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheatre(int id)
        {
            var deleted = await _theatreService.DeleteTheatreAsync(id);

            if (!deleted)
                return NotFound("Theatre not found.");

            return Ok("Theatre deleted successfully.");
        }
    }
}

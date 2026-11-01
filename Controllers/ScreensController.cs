using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpPost]
        public async Task<IActionResult> AddScreen(CreateScreenDto screenDto)
        {
            try
            {
                var result = await _screenService.AddScreenAsync(screenDto);
                return CreatedAtAction(nameof(GetScreenById),
                    new { id = result.ScreenId }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllScreens()
        {
            var result = await _screenService.GetAllScreensAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScreenById(int id)
        {
            var result = await _screenService.GetScreenByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Screen not found." });

            return Ok(result);
        }

        [HttpGet("theatre/{theatreId}")]
        public async Task<IActionResult> GetScreensByTheatre(int theatreId)
        {
            var result = await _screenService.GetScreensByTheatreIdAsync(theatreId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreen(int id, UpdateScreenDto screenDto)
        {
            try
            {
                var result = await _screenService.UpdateScreenAsync(id, screenDto);

                if (!result)
                    return NotFound(new { message = "Screen not found." });

                return Ok(new { message = "Screen updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            var result = await _screenService.DeleteScreenAsync(id);

            if (!result)
                return NotFound(new { message = "Screen not found." });

            return Ok(new { message = "Screen deleted successfully." });
        }
    }
}
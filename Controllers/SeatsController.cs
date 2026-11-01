using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSeat(CreateSeatDto seatDto)
        {
            try
            {
                var result = await _seatService.AddSeatAsync(seatDto);

                return CreatedAtAction(
                    nameof(GetSeatById),
                    new { id = result.SeatId },
                    result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddSeats(BulkCreateSeatDto seatDto)
        {
            try
            {
                var result = await _seatService.AddSeatsAsync(seatDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("screen/{screenId}")]
        public async Task<IActionResult> GetSeatsByScreen(int screenId)
        {
            try
            {
                var result = await _seatService.GetSeatsByScreenIdAsync(screenId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            var result = await _seatService.GetSeatByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Seat not found." });

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(int id, UpdateSeatDto seatDto)
        {
            try
            {
                var result = await _seatService.UpdateSeatAsync(id, seatDto);

                if (!result)
                    return NotFound(new { message = "Seat not found." });

                return Ok(new { message = "Seat updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var result = await _seatService.DeleteSeatAsync(id);

            if (!result)
                return NotFound(new { message = "Seat not found." });

            return Ok(new { message = "Seat deleted successfully." });
        }
    }
}

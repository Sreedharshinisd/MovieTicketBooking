
using MovieTicketBooking.DTOs.Theatres;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ITheatreService
    {
        Task<TheatreResponseDto> AddTheatreAsync(CreateTheatreDto theatreDto);

        Task<List<TheatreResponseDto>> GetAllTheatresAsync();

        Task<TheatreResponseDto?> GetTheatreByIdAsync(int theatreId);

        Task<bool> UpdateTheatreAsync(int theatreId, UpdateTheatreDto theatreDto);

        Task<bool> DeleteTheatreAsync(int theatreId);
    }
}

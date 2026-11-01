using MovieTicketBooking.DTOs.Screens;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IScreenService
    {
        Task<ScreenResponseDto> AddScreenAsync(CreateScreenDto screenDto);

        Task<List<ScreenResponseDto>> GetAllScreensAsync();

        Task<ScreenResponseDto?> GetScreenByIdAsync(int screenId);

        Task<List<ScreenResponseDto>> GetScreensByTheatreIdAsync(int theatreId);

        Task<bool> UpdateScreenAsync(int screenId, UpdateScreenDto screenDto);

        Task<bool> DeleteScreenAsync(int screenId);
    }
}
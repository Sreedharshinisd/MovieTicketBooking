using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;
        private readonly ITheatreRepository _theatreRepository;

        public ScreenService(
            IScreenRepository screenRepository,
            ITheatreRepository theatreRepository)
        {
            _screenRepository = screenRepository;
            _theatreRepository = theatreRepository;
        }

        public async Task<ScreenResponseDto> AddScreenAsync(CreateScreenDto screenDto)
        {
            if (string.IsNullOrWhiteSpace(screenDto.ScreenName))
                throw new ArgumentException("Screen name is required.");

            var theatre = await _theatreRepository.GetTheatreByIdAsync(screenDto.TheatreId);

            if (theatre == null)
                throw new ArgumentException("Theatre not found.");

            var screen = new Screen
            {
                TheatreId = screenDto.TheatreId,
                ScreenName = screenDto.ScreenName
            };

            var result = await _screenRepository.AddScreenAsync(screen);

            return MapToDto(result);
        }

        public async Task<List<ScreenResponseDto>> GetAllScreensAsync()
        {
            var screens = await _screenRepository.GetAllScreensAsync();

            return screens.Select(MapToDto).ToList();
        }

        public async Task<ScreenResponseDto?> GetScreenByIdAsync(int screenId)
        {
            var screen = await _screenRepository.GetScreenByIdAsync(screenId);

            return screen == null ? null : MapToDto(screen);
        }

        public async Task<List<ScreenResponseDto>> GetScreensByTheatreIdAsync(int theatreId)
        {
            var screens = await _screenRepository.GetScreensByTheatreIdAsync(theatreId);

            return screens.Select(MapToDto).ToList();
        }

        public async Task<bool> UpdateScreenAsync(int screenId, UpdateScreenDto screenDto)
        {
            if (string.IsNullOrWhiteSpace(screenDto.ScreenName))
                throw new ArgumentException("Screen name is required.");

            var screen = await _screenRepository.GetScreenByIdAsync(screenId);

            if (screen == null)
                return false;

            screen.ScreenName = screenDto.ScreenName;

            await _screenRepository.UpdateScreenAsync(screen);

            return true;
        }

        public async Task<bool> DeleteScreenAsync(int screenId)
        {
            var screen = await _screenRepository.GetScreenByIdAsync(screenId);

            if (screen == null)
                return false;

            await _screenRepository.DeleteScreenAsync(screen);

            return true;
        }

        private static ScreenResponseDto MapToDto(Screen screen)
        {
            return new ScreenResponseDto
            {
                ScreenId = screen.ScreenId,
                TheatreId = screen.TheatreId,
                ScreenName = screen.ScreenName
            };
        }
    }
}
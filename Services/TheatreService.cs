
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class TheatreService : ITheatreService
    {
        private readonly ITheatreRepository _theatreRepository;

        public TheatreService(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }

        public async Task<TheatreResponseDto> AddTheatreAsync(CreateTheatreDto theatreDto)
        {
            if (string.IsNullOrWhiteSpace(theatreDto.Name))
                throw new ArgumentException("Theatre name is required.");

            if (string.IsNullOrWhiteSpace(theatreDto.Location))
                throw new ArgumentException("Theatre location is required.");

            var theatre = new Theatre
            {
                Name = theatreDto.Name,
                Location = theatreDto.Location
            };

            var addedTheatre = await _theatreRepository.AddTheatreAsync(theatre);

            return new TheatreResponseDto
            {
                TheatreId = addedTheatre.TheatreId,
                Name = addedTheatre.Name,
                Location = addedTheatre.Location
            };
        }

        public async Task<List<TheatreResponseDto>> GetAllTheatresAsync()
        {
            var theatres = await _theatreRepository.GetAllTheatresAsync();

            return theatres.Select(t => new TheatreResponseDto
            {
                TheatreId = t.TheatreId,
                Name = t.Name,
                Location = t.Location
            }).ToList();
        }

        public async Task<TheatreResponseDto?> GetTheatreByIdAsync(int theatreId)
        {
            var theatre = await _theatreRepository.GetTheatreByIdAsync(theatreId);

            if (theatre == null)
                return null;

            return new TheatreResponseDto
            {
                TheatreId = theatre.TheatreId,
                Name = theatre.Name,
                Location = theatre.Location
            };
        }

        public async Task<bool> UpdateTheatreAsync(int theatreId, UpdateTheatreDto theatreDto)
        {
            if (string.IsNullOrWhiteSpace(theatreDto.Name))
                throw new ArgumentException("Theatre name is required.");

            if (string.IsNullOrWhiteSpace(theatreDto.Location))
                throw new ArgumentException("Theatre location is required.");

            var theatre = await _theatreRepository.GetTheatreByIdAsync(theatreId);

            if (theatre == null)
                return false;

            theatre.Name = theatreDto.Name;
            theatre.Location = theatreDto.Location;

            await _theatreRepository.UpdateTheatreAsync(theatre);

            return true;
        }

        public async Task<bool> DeleteTheatreAsync(int theatreId)
        {
            var theatre = await _theatreRepository.GetTheatreByIdAsync(theatreId);

            if (theatre == null)
                return false;

            await _theatreRepository.DeleteTheatreAsync(theatre);

            return true;
        }
    }
}

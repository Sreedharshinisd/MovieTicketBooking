using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IScreenRepository _screenRepository;
        private readonly ISeatRepository _seatRepository;

        public ShowService(
            IShowRepository showRepository,
            IMovieRepository movieRepository,
            IScreenRepository screenRepository,
            ISeatRepository seatRepository)
        {
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _screenRepository = screenRepository;
            _seatRepository = seatRepository;
        }

        public async Task<ShowResponseDto> CreateShowAsync(CreateShowDto showDto)
        {
            if (showDto.StartTime >= showDto.EndTime)
                throw new ArgumentException("End time must be after start time.");

            if (showDto.TicketPrice <= 0)
                throw new ArgumentException("Ticket price must be greater than zero.");

            var movie = await _movieRepository.GetMovieByIdAsync(showDto.MovieId);

            if (movie == null)
                throw new ArgumentException("Movie not found.");

            var screen = await _screenRepository.GetScreenByIdAsync(showDto.ScreenId);

            if (screen == null)
                throw new ArgumentException("Screen not found.");

            var existingShows = await _showRepository
                .GetShowsByScreenIdAsync(showDto.ScreenId);

            bool hasOverlap = existingShows.Any(s =>
                showDto.StartTime < s.EndTime &&
                showDto.EndTime > s.StartTime);

            if (hasOverlap)
                throw new ArgumentException("Another show is already scheduled during this time.");

            var seats = await _seatRepository
                .GetSeatsByScreenIdAsync(showDto.ScreenId);

            if (seats.Count == 0)
                throw new ArgumentException("No seats are configured for this screen.");

            var show = new Show
            {
                MovieId = showDto.MovieId,
                ScreenId = showDto.ScreenId,
                StartTime = showDto.StartTime,
                EndTime = showDto.EndTime,
                TicketPrice = showDto.TicketPrice
            };
            foreach (var seat in seats)
            {
                show.ShowSeats.Add(new ShowSeat
                {
                    SeatId = seat.SeatId,
                    Status = "Available"
                });
            }

            await _showRepository.AddShowAsync(show);

            return MapToDto(show);
        }

        public async Task<List<ShowResponseDto>> GetAllShowsAsync()
        {
            var shows = await _showRepository.GetAllShowsAsync();
            return shows.Select(MapToDto).ToList();
        }

        public async Task<ShowResponseDto?> GetShowByIdAsync(int showId)
        {
            var show = await _showRepository.GetShowByIdAsync(showId);
            return show == null ? null : MapToDto(show);
        }

        public async Task<List<ShowResponseDto>> GetShowsByMovieIdAsync(int movieId)
        {
            var shows = await _showRepository.GetShowsByMovieIdAsync(movieId);
            return shows.Select(MapToDto).ToList();
        }

        public async Task<List<ShowResponseDto>> GetShowsByScreenIdAsync(int screenId)
        {
            var shows = await _showRepository.GetShowsByScreenIdAsync(screenId);
            return shows.Select(MapToDto).ToList();
        }

        public async Task<bool> UpdateShowAsync(int showId, UpdateShowDto showDto)
        {
            var show = await _showRepository.GetShowByIdAsync(showId);

            if (show == null)
                return false;

            if (showDto.StartTime >= showDto.EndTime)
                throw new ArgumentException("End time must be after start time.");

            if (showDto.TicketPrice <= 0)
                throw new ArgumentException("Ticket price must be greater than zero.");

            var existingShows = await _showRepository
                .GetShowsByScreenIdAsync(show.ScreenId);

            bool hasOverlap = existingShows.Any(s =>
                s.ShowId != showId &&
                showDto.StartTime < s.EndTime &&
                showDto.EndTime > s.StartTime);

            if (hasOverlap)
                throw new ArgumentException("Another show is already scheduled during this time.");

            show.StartTime = showDto.StartTime;
            show.EndTime = showDto.EndTime;
            show.TicketPrice = showDto.TicketPrice;

            await _showRepository.UpdateShowAsync(show);

            return true;
        }

        public async Task<bool> DeleteShowAsync(int showId)
        {
            var show = await _showRepository.GetShowByIdAsync(showId);

            if (show == null)
                return false;

            await _showRepository.DeleteShowAsync(show);

            return true;
        }

        public async Task<List<ShowSeatResponseDto>> GetShowSeatsAsync(int showId)
        {
            return await GetSeatsByStatusAsync(showId, null);
        }

        public async Task<List<ShowSeatResponseDto>> GetAvailableSeatsAsync(int showId)
        {
            return await GetSeatsByStatusAsync(showId, "Available");
        }

        public async Task<List<ShowSeatResponseDto>> GetBookedSeatsAsync(int showId)
        {
            return await GetSeatsByStatusAsync(showId, "Booked");
        }

        private async Task<List<ShowSeatResponseDto>> GetSeatsByStatusAsync(
            int showId, string? status)
        {
            var show = await _showRepository.GetShowByIdAsync(showId);

            if (show == null)
                throw new ArgumentException("Show not found.");

            var result = show.ShowSeats
                .Where(s => status == null || s.Status == status)
                .Select(s => new ShowSeatResponseDto
                {
                    ShowSeatId = s.ShowSeatId,
                    ShowId = s.ShowId,
                    SeatId = s.SeatId,
                    RowName = s.Seat.RowName,
                    SeatNumber = s.Seat.SeatNumber,
                    SeatType = s.Seat.SeatType,
                    Status = s.Status
                })
                .ToList();

            return result;
        }

        private static ShowResponseDto MapToDto(Show show)
        {
            return new ShowResponseDto
            {
                ShowId = show.ShowId,
                MovieId = show.MovieId,
                ScreenId = show.ScreenId,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TicketPrice = show.TicketPrice
            };
        }
    }
}
using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IShowService
    {
        Task<ShowResponseDto> CreateShowAsync(CreateShowDto showDto);

        Task<List<ShowResponseDto>> GetAllShowsAsync();

        Task<ShowResponseDto?> GetShowByIdAsync(int showId);

        Task<List<ShowResponseDto>> GetShowsByMovieIdAsync(int movieId);

        Task<List<ShowResponseDto>> GetShowsByScreenIdAsync(int screenId);

        Task<bool> UpdateShowAsync(int showId, UpdateShowDto showDto);

        Task<bool> DeleteShowAsync(int showId);

        Task<List<ShowSeatResponseDto>> GetShowSeatsAsync(int showId);

        Task<List<ShowSeatResponseDto>> GetAvailableSeatsAsync(int showId);

        Task<List<ShowSeatResponseDto>> GetBookedSeatsAsync(int showId);
    }
}
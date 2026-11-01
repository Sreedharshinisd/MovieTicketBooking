using MovieTicketBooking.DTOs.Seats;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ISeatService
    {
        Task<SeatResponseDto> AddSeatAsync(CreateSeatDto seatDto);
        Task<List<SeatResponseDto>> AddSeatsAsync(BulkCreateSeatDto seatDto);
        Task<List<SeatResponseDto>> GetSeatsByScreenIdAsync(int screenId);
        Task<SeatResponseDto?> GetSeatByIdAsync(int seatId);
        Task<bool> UpdateSeatAsync(int seatId, UpdateSeatDto seatDto);
        Task<bool> DeleteSeatAsync(int seatId);
    }
}

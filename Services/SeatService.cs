using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IScreenRepository _screenRepository;

        public SeatService(
            ISeatRepository seatRepository,
            IScreenRepository screenRepository)
        {
            _seatRepository = seatRepository;
            _screenRepository = screenRepository;
        }

        public async Task<SeatResponseDto> AddSeatAsync(CreateSeatDto seatDto)
        {
            ValidateSeat(seatDto.RowName, seatDto.SeatNumber, seatDto.SeatType);

            var screen = await _screenRepository.GetScreenByIdAsync(seatDto.ScreenId);

            if (screen == null)
                throw new ArgumentException("Screen not found.");

            var seat = new Seat
            {
                ScreenId = seatDto.ScreenId,
                RowName = seatDto.RowName,
                SeatNumber = seatDto.SeatNumber,
                SeatType = seatDto.SeatType
            };

            var result = await _seatRepository.AddSeatAsync(seat);
            return MapToDto(result);
        }

        public async Task<List<SeatResponseDto>> AddSeatsAsync(BulkCreateSeatDto seatDto)
        {
            if (seatDto.Seats == null || seatDto.Seats.Count == 0)
                throw new ArgumentException("At least one seat is required.");

            var screen = await _screenRepository.GetScreenByIdAsync(seatDto.ScreenId);

            if (screen == null)
                throw new ArgumentException("Screen not found.");

            foreach (var item in seatDto.Seats)
            {
                ValidateSeat(item.RowName, item.SeatNumber, item.SeatType);
            }

            var duplicateSeats = seatDto.Seats
                .GroupBy(s => new { s.RowName, s.SeatNumber })
                .Any(g => g.Count() > 1);

            if (duplicateSeats)
                throw new ArgumentException("Duplicate seats in the request.");

            var existingSeats = await _seatRepository.GetSeatsByScreenIdAsync(seatDto.ScreenId);

            foreach (var item in seatDto.Seats)
            {
                if (existingSeats.Any(s =>
                    s.RowName == item.RowName && s.SeatNumber == item.SeatNumber))
                {
                    throw new ArgumentException(
                        $"Seat {item.RowName}{item.SeatNumber} already exists.");
                }
            }

            var seats = seatDto.Seats.Select(item => new Seat
            {
                ScreenId = seatDto.ScreenId,
                RowName = item.RowName,
                SeatNumber = item.SeatNumber,
                SeatType = item.SeatType
            }).ToList();

            var result = await _seatRepository.AddSeatsAsync(seats);
            return result.Select(MapToDto).ToList();
        }

        public async Task<List<SeatResponseDto>> GetSeatsByScreenIdAsync(int screenId)
        {
            var screen = await _screenRepository.GetScreenByIdAsync(screenId);

            if (screen == null)
                throw new ArgumentException("Screen not found.");

            var seats = await _seatRepository.GetSeatsByScreenIdAsync(screenId);
            return seats.Select(MapToDto).ToList();
        }

        public async Task<SeatResponseDto?> GetSeatByIdAsync(int seatId)
        {
            var seat = await _seatRepository.GetSeatByIdAsync(seatId);
            return seat == null ? null : MapToDto(seat);
        }

        public async Task<bool> UpdateSeatAsync(int seatId, UpdateSeatDto seatDto)
        {
            ValidateSeat(seatDto.RowName, seatDto.SeatNumber, seatDto.SeatType);

            var seat = await _seatRepository.GetSeatByIdAsync(seatId);

            if (seat == null)
                return false;

            seat.RowName = seatDto.RowName;
            seat.SeatNumber = seatDto.SeatNumber;
            seat.SeatType = seatDto.SeatType;

            await _seatRepository.UpdateSeatAsync(seat);
            return true;
        }

        public async Task<bool> DeleteSeatAsync(int seatId)
        {
            var seat = await _seatRepository.GetSeatByIdAsync(seatId);

            if (seat == null)
                return false;

            await _seatRepository.DeleteSeatAsync(seat);
            return true;
        }

        private static void ValidateSeat(string rowName, int seatNumber, string seatType)
        {
            if (string.IsNullOrWhiteSpace(rowName))
                throw new ArgumentException("Row name is required.");

            if (seatNumber <= 0)
                throw new ArgumentException("Seat number must be greater than zero.");

            if (string.IsNullOrWhiteSpace(seatType))
                throw new ArgumentException("Seat type is required.");
        }

        private static SeatResponseDto MapToDto(Seat seat)
        {
            return new SeatResponseDto
            {
                SeatId = seat.SeatId,
                ScreenId = seat.ScreenId,
                RowName = seat.RowName,
                SeatNumber = seat.SeatNumber,
                SeatType = seat.SeatType
            };
        }
    }
}

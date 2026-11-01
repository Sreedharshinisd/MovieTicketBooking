namespace MovieTicketBooking.DTOs.Seats
{
    public class BulkCreateSeatDto
    {
        public int ScreenId { get; set; }
        public List<SeatItemDto> Seats { get; set; } = new();
    }

    public class SeatItemDto
    {
        public string RowName { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public string SeatType { get; set; } = string.Empty;
    }
}

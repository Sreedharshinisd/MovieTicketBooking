namespace MovieTicketBooking.DTOs.Bookings
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int ShowId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<int> SeatIds { get; set; } = new();
    }
}

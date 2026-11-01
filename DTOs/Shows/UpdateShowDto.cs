namespace MovieTicketBooking.DTOs.Shows
{
    public class UpdateShowDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal TicketPrice { get; set; }
    }
}

namespace MovieTicketBooking.DTOs.Movies
{
    public class UpdateMovieDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public string Language { get; set; } = string.Empty;

        public DateOnly ReleaseDate { get; set; }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;
namespace MovieTicketBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database tables
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<ShowSeat> ShowSeats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>();
            // MOVIE
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);

                entity.Property(m => m.Title)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(m => m.Description)
                    .HasMaxLength(500);

                entity.Property(m => m.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.Genre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.ReleaseDate)
                    .HasColumnType("date");

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_Movies_Duration",
                        "[DurationMinutes] > 0"));
            });

            // THEATRE
            modelBuilder.Entity<Theatre>(entity =>
            {
                entity.HasKey(t => t.TheatreId);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(t => t.Location)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            // SCREEN
            modelBuilder.Entity<Screen>(entity =>
            {
                entity.HasKey(s => s.ScreenId);

                entity.Property(s => s.ScreenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
                    .IsUnique();

                entity.HasOne(s => s.Theatre)
                    .WithMany(t => t.Screens)
                    .HasForeignKey(s => s.TheatreId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SEAT
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(s => s.SeatId);

                entity.Property(s => s.RowName)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(s => s.SeatType)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasIndex(s => new
                {
                    s.ScreenId,
                    s.RowName,
                    s.SeatNumber
                }).IsUnique();

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_Seats_SeatNumber",
                        "[SeatNumber] > 0"));

                entity.HasOne(s => s.Screen)
                    .WithMany(sc => sc.Seats)
                    .HasForeignKey(s => s.ScreenId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SHOW
            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasKey(s => s.ShowId);

                entity.Property(s => s.TicketPrice)
                    .HasPrecision(10, 2);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Shows_Time",
                        "[EndTime] > [StartTime]");

                    t.HasCheckConstraint(
                        "CK_Shows_TicketPrice",
                        "[TicketPrice] >= 0");
                });

                entity.HasOne(s => s.Movie)
                    .WithMany(m => m.Shows)
                    .HasForeignKey(s => s.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Screen)
                    .WithMany(sc => sc.Shows)
                    .HasForeignKey(s => s.ScreenId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SHOW SEAT
            modelBuilder.Entity<ShowSeat>(entity =>
            {
                entity.HasKey(s => s.ShowSeatId);

                entity.Property(s => s.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(s => new { s.ShowId, s.SeatId })
                    .IsUnique();

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_ShowSeats_Status",
                        "[Status] IN ('Available', 'Booked')"));

                entity.HasOne(s => s.Show)
                    .WithMany(sh => sh.ShowSeats)
                    .HasForeignKey(s => s.ShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Seat)
                    .WithMany(se => se.ShowSeats)
                    .HasForeignKey(s => s.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // BOOKING
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.BookingReference)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasIndex(b => b.BookingReference)
                    .IsUnique();

                entity.Property(b => b.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.CustomerEmail)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(b => b.CustomerPhone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(b => b.TotalAmount)
                    .HasPrecision(10, 2);

                entity.Property(b => b.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Bookings_TotalAmount",
                        "[TotalAmount] >= 0");

                    t.HasCheckConstraint(
                        "CK_Bookings_Status",
                        "[Status] IN ('Confirmed', 'Cancelled')");
                });

                entity.HasOne(b => b.Show)
                    .WithMany(s => s.Bookings)
                    .HasForeignKey(b => b.ShowId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // BOOKING SEAT
            modelBuilder.Entity<BookingSeat>(entity =>
            {
                entity.HasKey(b => b.BookingSeatId);

                entity.Property(b => b.Price)
                    .HasPrecision(10, 2);

                entity.ToTable(t =>
                    t.HasCheckConstraint(
                        "CK_BookingSeats_Price",
                        "[Price] >= 0"));

                entity.HasOne(b => b.Booking)
                    .WithMany(booking => booking.BookingSeats)
                    .HasForeignKey(b => b.BookingId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Seat)
                    .WithMany(s => s.BookingSeats)
                    .HasForeignKey(b => b.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
          
        }
    }
}

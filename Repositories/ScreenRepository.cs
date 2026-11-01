using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Screen> AddScreenAsync(Screen screen)
        {
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();
            return screen;
        }

        public async Task<List<Screen>> GetAllScreensAsync()
        {
            return await _context.Screens.ToListAsync();
        }

        public async Task<Screen?> GetScreenByIdAsync(int screenId)
        {
            return await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenId == screenId);
        }

        public async Task<List<Screen>> GetScreensByTheatreIdAsync(int theatreId)
        {
            return await _context.Screens
                .Where(s => s.TheatreId == theatreId)
                .ToListAsync();
        }

        public async Task UpdateScreenAsync(Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteScreenAsync(Screen screen)
        {
            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();
        }
    }
}
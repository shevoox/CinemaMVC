using CinemaMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaMVC.Repositories
{
    public class TheaterRepository : Repository<Theater>, ITheaterRepository
    {
        private readonly ApplicationDbContext _context;

        public TheaterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Theater>> GetAllTheatersAsync()
        {
            return await _context.Theaters
                .Include(t => t.Showtimes)
                .Include(t => t.Seats)
                .ToListAsync();
        }

        public async Task<Theater> GetTheaterByIdAsync(int id)
        {
            return await _context.Theaters
                .FindAsync(id); 
        }

        public async Task<Theater> GetTheaterWithShowtimesAsync(int id)
        {
            return await _context.Theaters
                .Include(t => t.Showtimes)
                .ThenInclude(s => s.Movie)
                .Include(t => t.Seats)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTheaterAsync(Theater theater)
        {
            await _context.Theaters.AddAsync(theater);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTheaterAsync(Theater theater)
        {
            _context.Theaters.Update(theater);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTheaterAsync(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater != null)
            {
                _context.Theaters.Remove(theater);
                await _context.SaveChangesAsync();
            }
        }
    }
} 
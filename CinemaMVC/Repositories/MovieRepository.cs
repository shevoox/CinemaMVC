using CinemaMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaMVC.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public async Task<IEnumerable<Movie>> GetTopRatedAsync(int count)
        {
            return await _context.Movies
                .OrderByDescending(m => m.Rating).Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre).Include(m => m.Showtimes)
                 .ThenInclude(s => s.Theater)
                   .ThenInclude(t => t.Seats)

                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetPopularAsync(int count)
        {
            return await _context.Movies
                .Where(m => m.IsFeatured == true).Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre).Include(m => m.Showtimes)
                 .ThenInclude(s => s.Theater)
                   .ThenInclude(t => t.Seats)
                .Take(count)
                .ToListAsync();
        }
        public async Task<IEnumerable<Movie>> GetAllMovieAsync()
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre).Include(m => m.Showtimes).ThenInclude(b => b.Bookings).Include(m => m.Showtimes)
                 .ThenInclude(s => s.Theater)
                   .ThenInclude(t => t.Seats)
                .ToListAsync();
        }
        public async Task<Movie> GetMovieWithGenres(int id)
        {
            return await _context.Movies.Include(e => e.MovieCasts).ThenInclude(e => e.Cast)
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre).Include(m => m.Showtimes).ThenInclude(b => b.Bookings).Include(m => m.Showtimes)
                 .ThenInclude(s => s.Theater)
                   .ThenInclude(t => t.Seats)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie> BookingMovie(int id)
        {
            return await _context.Movies.Include(e => e.MovieCasts).ThenInclude(e => e.Cast)
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre).Include(m => m.Showtimes).ThenInclude(b => b.Bookings).Include(m => m.Showtimes)
                 .ThenInclude(s => s.Theater)
                   .ThenInclude(t => t.Seats)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Movie>> SearchAMovie(string name)
        {
            return await _context.Movies
                .Include(e => e.MovieCasts).ThenInclude(e => e.Cast)
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .Include(m => m.Showtimes).ThenInclude(b => b.Bookings)
                .Include(m => m.Showtimes).ThenInclude(s => s.Theater).ThenInclude(t => t.Seats)
                .Where(m => m.Title.Contains(name))
                .ToListAsync();
        }


    }
}

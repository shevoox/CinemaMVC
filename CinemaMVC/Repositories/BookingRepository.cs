using CinemaMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaMVC.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Showtime)
                .ThenInclude(s => s.Movie)
                .Include(b => b.Showtime)
                .ThenInclude(s => s.Theater)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetRecentBookingsAsync(int count)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Showtime)
                .ThenInclude(s => s.Movie)
                .Include(b => b.Showtime)
                .ThenInclude(s => s.Theater)
                .OrderByDescending(b => b.BookingDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Showtime)
                .ThenInclude(s => s.Movie)
                .Include(b => b.Showtime)
                .ThenInclude(s => s.Theater)
                .Include(b => b.Seats)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> GetTodayBookingsCountAsync()
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            
            return await _context.Bookings
                .Where(b => b.BookingDate >= today && b.BookingDate < tomorrow)
                .CountAsync();
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            
            return await _context.Bookings
                .Where(b => b.BookingDate >= today && b.BookingDate < tomorrow && b.IsPaid)
                .SumAsync(b => b.TotalPrice);
        }
    }
} 
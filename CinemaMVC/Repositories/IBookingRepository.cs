using CinemaMVC.Models;

namespace CinemaMVC.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetRecentBookingsAsync(int count);
        Task<Booking> GetBookingByIdAsync(int id);
        Task<int> GetTodayBookingsCountAsync();
        Task<decimal> GetTodayRevenueAsync();
    }
} 
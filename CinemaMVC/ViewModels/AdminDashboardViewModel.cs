using CinemaMVC.Models;

namespace CinemaMVC.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public int TotalMovies { get; set; }
        public int TotalTheaters { get; set; }
        public int TodayBookingsCount { get; set; }
        public decimal TodayRevenue { get; set; }
        public IEnumerable<Booking> RecentBookings { get; set; }

        public AdminDashboardViewModel()
        {
            Movies = new List<Movie>();
            RecentBookings = new List<Booking>();
        }
    }
} 
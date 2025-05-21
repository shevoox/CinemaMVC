using CinemaMVC.Models;
using CinemaMVC.Models.ViewModels;
using CinemaMVC.Repositories;
using CinemaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace CinemaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Movie> _movieRepo;
        private readonly IMovieRepository _movieRepository;
        public HomeController(ILogger<HomeController> logger, IRepository<Movie> movieRepo, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepo = movieRepo;
            _movieRepository = movieRepository;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var topRated = await _movieRepository.GetTopRatedAsync(10);
            var popular = await _movieRepository.GetPopularAsync(10);
            var AllMovies = await _movieRepository.GetAllMovieAsync();
            var todayShowings = AllMovies
            .Where(m => m.Showtimes != null &&
                       m.Showtimes.Any(s => s.StartTime.Date == today))
            .Select(m => new TodayShowingVM
            {
                Id = m.Id,
                Title = m.Title,
                PosterImage = m.PosterImage,
                Duration = m.DurationMinutes,
                Rating = m.Rating,
                Format = m.Format,
                Theater = m.Showtimes.First(s => s.StartTime.Date == today).Theater?.Name ?? "N/A",
                AvailableSeats = m.Showtimes.First(s => s.StartTime.Date == today).AvailableSeats,
                NextShowtime = m.Showtimes
                    .Where(s => s.StartTime.Date == today)
                    .OrderBy(s => s.StartTime)
                    .First().FormattedStartTime
            })
            .OrderBy(m => m.NextShowtime)
            .Take(4)
            .ToList();



            var viewModel = new HomeViewModel
            {
                AllMovies = AllMovies,
                TopRatedMovies = topRated,
                PopularMovies = popular,
                TodayShowings = todayShowings
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

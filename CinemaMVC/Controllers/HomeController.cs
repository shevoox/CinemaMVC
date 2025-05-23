using CinemaMVC.Models;
using CinemaMVC.Models.ViewModels;
using CinemaMVC.Repositories;
using CinemaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CinemaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Movie> _movieRepo;
        private readonly IMovieRepository _movieRepository;
        private readonly ApplicationDbContext _context;
        private const int PageSize = 12; // عدد العناصر في كل صفحة

        public HomeController(
            ILogger<HomeController> logger, 
            IRepository<Movie> movieRepo, 
            IMovieRepository movieRepository,
            ApplicationDbContext context)
        {
            _logger = logger;
            _movieRepo = movieRepo;
            _movieRepository = movieRepository;
            _context = context;
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

        public async Task<IActionResult> Movies(int page = 1)
        {
            var allMovies = await _movieRepository.GetAllMovieAsync();
            var totalMovies = allMovies.Count();
            var totalPages = (int)Math.Ceiling(totalMovies / (double)PageSize);

            // التأكد من أن رقم الصفحة صالح
            page = Math.Max(1, Math.Min(page, totalPages));

            var movies = allMovies
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // Get user's favorite movies if logged in
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var favoriteMovies = await _movieRepository.GetUserFavoriteMoviesAsync(userId);
                ViewBag.FavoriteMovies = favoriteMovies.Select(m => m.Id).ToList();
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;

            return View(movies);
        }

        public async Task<IActionResult> TVShows(int page = 1)
        {
            var allShows = await _movieRepository.GetAllMovieAsync();
            var tvShows = allShows.Where(m => m.Format == "TV Show");
            var totalShows = tvShows.Count();
            var totalPages = (int)Math.Ceiling(totalShows / (double)PageSize);

            page = Math.Max(1, Math.Min(page, totalPages));

            var shows = tvShows
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;

            return View(shows);
        }

        public async Task<IActionResult> NewAndPopular(int page = 1)
        {
            var allMovies = await _movieRepository.GetAllMovieAsync();
            var newMovies = allMovies.OrderByDescending(m => m.ReleaseYear);
            var totalMovies = newMovies.Count();
            var totalPages = (int)Math.Ceiling(totalMovies / (double)PageSize);

            page = Math.Max(1, Math.Min(page, totalPages));

            var movies = newMovies
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // Get user's favorite movies if logged in
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var favoriteMovies = await _movieRepository.GetUserFavoriteMoviesAsync(userId);
                ViewBag.FavoriteMovies = favoriteMovies.Select(m => m.Id).ToList();
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;

            return View(movies);
        }

        //[Authorize]
        public async Task<IActionResult> MyList(int page = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoriteMovies = await _movieRepository.GetUserFavoriteMoviesAsync(userId);
            var totalMovies = favoriteMovies.Count();
            var totalPages = (int)Math.Ceiling(totalMovies / (double)PageSize);

            page = Math.Max(1, Math.Min(page, totalPages));

            var movies = favoriteMovies
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;

            return View(movies);
        }


        public IActionResult About()
        {
            return View();
        }

        public IActionResult Theaters()
        {
            return View();
        }

        public IActionResult Careers()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int movieId, string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Register", "Register");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoriteMovies = await _movieRepository.GetUserFavoriteMoviesAsync(userId);
            var isFavorite = favoriteMovies.Any(m => m.Id == movieId);

            if (isFavorite)
            {
                // Remove from favorites
                var favorite = _context.UserFavorites.FirstOrDefault(f => f.UserId == userId && f.MovieId == movieId);
                if (favorite != null)
                {
                    _context.UserFavorites.Remove(favorite);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                // Add to favorites
                var newFavorite = new UserFavorite
                {
                    UserId = userId,
                    MovieId = movieId,
                    DateAdded = DateTime.UtcNow
                };
                _context.UserFavorites.Add(newFavorite);
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }


            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

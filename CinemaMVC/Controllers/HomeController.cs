using CinemaMVC.Models;
using CinemaMVC.Models.ViewModels;
using CinemaMVC.Repositories;
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
            var topRated = await _movieRepository.GetTopRatedAsync(10);
            var popular = await _movieRepository.GetPopularAsync(10);
            var AllMovies = await _movieRepository.GetAllMovieAsync();

            var viewModel = new HomeViewModel
            {
                AllMovies = AllMovies,
                TopRatedMovies = topRated,
                PopularMovies = popular
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

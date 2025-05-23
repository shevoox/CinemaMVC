using CinemaMVC.Models;
using CinemaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaMVC.Controllers
{
    public class DetailsController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ApplicationDbContext _context;

        public DetailsController(IMovieRepository movieRepository, ApplicationDbContext context)
        {
            _movieRepository = movieRepository;
            _context = context;
        }

        public async Task<IActionResult> details(int id)
        {
            Movie movie = await _movieRepository.GetMovieWithGenres(id);

            // Get user's favorite movies if logged in
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var favoriteMovies = await _movieRepository.GetUserFavoriteMoviesAsync(userId);
                ViewBag.FavoriteMovies = favoriteMovies.Select(m => m.Id).ToList();
            }

            return View(movie);
        }
    }
}

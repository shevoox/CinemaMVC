using CinemaMVC.Models;
using CinemaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMVC.Controllers
{
    public class DetailsController : Controller
    {

        private readonly IMovieRepository _movieRepository;
        public DetailsController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<IActionResult> details(int id)
        {
            Movie movie = await _movieRepository.GetMovieWithGenres(id);
            return View(movie);
        }
    }
}

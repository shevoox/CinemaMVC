using CinemaMVC.Models;
using CinemaMVC.Models.ViewModels;
using CinemaMVC.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace CinemaMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRepository<Movie> _repository;
        private readonly IMovieRepository _movieRepository;
        public SearchController(IRepository<Movie> repository, IMovieRepository movieRepository)
        {
            _repository = repository;
            _movieRepository = movieRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Search(string FilmName)
        {
            var movies = await _movieRepository.SearchAMovie(FilmName);
            if (movies == null || !movies.Any())
            {
                // تحويل لصفحة تانية اسمها NoResults.cshtml
                return View("NoResults", FilmName); // ممكن تبعت كلمة البحث
            }

            var searchResult = new SearchViewModel
            {
                Results = movies,
                SearchTerm = FilmName
            };

            return View(searchResult);
        }
        public IActionResult NoResults(string FilmName)
        {
            return View(FilmName);
        }
    }
}

using CinemaMVC.Models;
using CinemaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly IRepository<Movie> _repository;
        private readonly IMovieRepository _movieRepository;
        public BookingController(IRepository<Movie> repository, Repositories.IMovieRepository movieRepository)
        {
            _repository = repository;
            _movieRepository = movieRepository;
        }
        public async Task<IActionResult> Booking(int id)
        {
            var bookingMovie = await _movieRepository.BookingMovie(id);
            return View(bookingMovie);
        }
    }
}

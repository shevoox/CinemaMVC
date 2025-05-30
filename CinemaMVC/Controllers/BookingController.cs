using CinemaMVC.Models;
using CinemaMVC.Models.ViewModels;
using CinemaMVC.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly IRepository<Movie> _repository;
        private readonly IMovieRepository _movieRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(IRepository<Movie> repository, Repositories.IMovieRepository movieRepository, UserManager<ApplicationUser> userManager,
            IBookingRepository bookingRepository)
        {
            _repository = repository;
            _movieRepository = movieRepository;
            _userManager = userManager;
            _bookingRepository = bookingRepository;
        }

        public IBookingRepository BookingRepository { get; }

        public async Task<IActionResult> Booking(int id)
        {

            var movie = await _movieRepository.BookingMovie(id);
            if (movie == null) return NotFound();

            var firstShowtime = movie.Showtimes.FirstOrDefault();
            if (firstShowtime == null) return BadRequest("No showtimes available for this movie.");

            var vm = new BookingVM
            {
                Movie = movie,
                Showtime = firstShowtime,
                Seats = firstShowtime.Theater.Seats.ToList(),
                IsSoldOut = false,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(BookingVM model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Register", "Account");
            }
            var seatsArray = model.SelectedSeats[0]
    .Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (model.SelectedSeats == null || !model.SelectedSeats.Any())
            {
                ModelState.AddModelError("", "Please select at least one seat.");
                return View("Booking", model);
            }
            var movie = await _movieRepository.BookingMovie(model.MovieId);
            var SelectedMovieSeats = movie.Showtimes.FirstOrDefault()?.Theater.Seats.ToList();
            if (SelectedMovieSeats.Count() > 0)
            {
                var paymentVM = new PaymentVM
                {

                    UserId = user.Id,
                    ShowtimeId = model.ShowtimeId,
                    NumberOfSeats = seatsArray.Length,
                    TotalPrice = model.TotalPrice,
                    IsPaid = false,
                    MovieTitle = movie.Title,
                    BookingDate = DateTime.Now,

                };
                return RedirectToAction("Payment", "Payment", paymentVM);
            }
            return View(model);

        }
    }
}

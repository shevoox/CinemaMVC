using CinemaMVC.Models;
using CinemaMVC.Models.ViewModels;
using CinemaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaMVC.Controllers.dashboard
{
    public class AdminController : Controller
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly ITheaterRepository _theaterRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMovieRepository _movieDetailRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Director> _directorRepository;
        private readonly IRepository<Cast> _castRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public AdminController(
            IRepository<Movie> movieRepository,
            ITheaterRepository theaterRepository,
            IBookingRepository bookingRepository,
            IMovieRepository movieDetailRepository,
            IRepository<Genre> genreRepository,
            IRepository<Director> directorRepository,
            IRepository<Cast> castRepository,
            IWebHostEnvironment webHostEnvironment,
            ApplicationDbContext context)
        {
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
            _bookingRepository = bookingRepository;
            _movieDetailRepository = movieDetailRepository;
            _genreRepository = genreRepository;
            _directorRepository = directorRepository;
            _castRepository = castRepository;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Admin()
        {
            var movies = await _movieRepository.GetAllAsync();
            var theaters = await _theaterRepository.GetAllTheatersAsync();
            var recentBookings = await _bookingRepository.GetRecentBookingsAsync(5);
            var todayBookingsCount = await _bookingRepository.GetTodayBookingsCountAsync();
            var todayRevenue = await _bookingRepository.GetTodayRevenueAsync();

            var viewModel = new AdminDashboardViewModel
            {
                Movies = movies,
                TotalMovies = movies.Count(),
                TotalTheaters = theaters.Count(),
                TodayBookingsCount = todayBookingsCount,
                TodayRevenue = todayRevenue,
                RecentBookings = recentBookings
            };

            return View(viewModel);
        }

        public async Task<IActionResult> MoviesManagement()
        {
            var movies = await _movieDetailRepository.GetAllMovieAsync();
            return View(movies);
        }

        public async Task<IActionResult> AddMovie()
        {
            var genres = await _genreRepository.GetAllAsync();
            var directors = await _directorRepository.GetAllAsync();
            var theaters = await _theaterRepository.GetAllTheatersAsync();
            var casts = await _castRepository.GetAllAsync();

            var viewModel = new MovieFormViewModel
            {
                Genres = genres.ToList(),
                Directors = directors.ToList(),
                Theaters = theaters.ToList(),
                Casts = casts.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(MovieFormViewModel viewModel)
        {
            // Remove PosterImage from ModelState validation since it's populated from PosterFile
            ModelState.Remove("PosterImage");

            if (ModelState.IsValid)
            {
                // Handle file upload
                string posterImagePath = null;
                if (viewModel.PosterFile != null && viewModel.PosterFile.Length > 0)
                {
                    // Create uploads directory if it doesn't exist
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.PosterFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.PosterFile.CopyToAsync(fileStream);
                    }

                    // Store only the filename in the database
                    posterImagePath = uniqueFileName;
                }

                var movie = new Movie
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    DurationMinutes = viewModel.DurationMinutes,
                    ReleaseYear = viewModel.ReleaseYear,
                    Rating = viewModel.Rating,
                    Format = viewModel.Format,
                    HasSubtitles = viewModel.HasSubtitles,
                    IsFeatured = viewModel.IsFeatured,
                    PosterImage = posterImagePath ?? viewModel.PosterImage ?? "https://via.placeholder.com/300x450",
                    TrailerUrl = viewModel.TrailerUrl,
                    Price = viewModel.Price,
                    Language = viewModel.Language,
                    DirectorId = viewModel.DirectorId
                };

                await _movieRepository.AddAsync(movie);
                await _context.SaveChangesAsync();

                // Add movie genres
                if (viewModel.SelectedGenreIds != null && viewModel.SelectedGenreIds.Any())
                {
                    foreach (var genreId in viewModel.SelectedGenreIds)
                    {
                        var movieGenre = new MovieGenre
                        {
                            MovieId = movie.Id,
                            GenreId = genreId
                        };
                        _context.MovieGenres.Add(movieGenre);
                    }
                    await _context.SaveChangesAsync();
                }

                // Add movie cast
                if (viewModel.SelectedCastIds != null && viewModel.SelectedCastIds.Any())
                {
                    foreach (var castId in viewModel.SelectedCastIds)
                    {
                        var movieCast = new MovieCast
                        {
                            MovieId = movie.Id,
                            CastId = castId
                        };
                        _context.MovieCasts.Add(movieCast);
                    }
                    await _context.SaveChangesAsync();
                }

                // Add movie showtimes for selected theaters
                if (viewModel.SelectedTheaterIds != null && viewModel.SelectedTheaterIds.Any())
                {
                    foreach (var theaterId in viewModel.SelectedTheaterIds)
                    {
                        // Create a default showtime for each theater
                        var showtime = new Showtime
                        {
                            MovieId = movie.Id,
                            TheaterId = theaterId,
                            StartTime = DateTime.Now.AddDays(1).Date.AddHours(18), // Default to tomorrow at 6 PM
                            EndTime = DateTime.Now.AddDays(1).Date.AddHours(18 + (movie.DurationMinutes / 60.0))
                        };
                        _context.Showtimes.Add(showtime);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(MoviesManagement));
            }

            // If we got this far, something failed, redisplay form
            viewModel.Genres = (await _genreRepository.GetAllAsync()).ToList();
            viewModel.Directors = (await _directorRepository.GetAllAsync()).ToList();
            viewModel.Theaters = (await _theaterRepository.GetAllTheatersAsync()).ToList();
            viewModel.Casts = (await _castRepository.GetAllAsync()).ToList();
            return View(viewModel);
        }

        public async Task<IActionResult> EditMovie(int id)
        {
            var movie = await _movieDetailRepository.GetMovieWithGenres(id);
            if (movie == null)
            {
                return NotFound();
            }

            var genres = await _genreRepository.GetAllAsync();
            var directors = await _directorRepository.GetAllAsync();
            var theaters = await _theaterRepository.GetAllTheatersAsync();
            var casts = await _castRepository.GetAllAsync();

            var viewModel = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseYear = movie.ReleaseYear,
                Rating = movie.Rating,
                Format = movie.Format,
                HasSubtitles = movie.HasSubtitles,
                IsFeatured = movie.IsFeatured,
                PosterImage = movie.PosterImage,
                TrailerUrl = movie.TrailerUrl,
                Price = movie.Price,
                Language = movie.Language,
                DirectorId = movie.DirectorId,
                Genres = genres.ToList(),
                Directors = directors.ToList(),
                Theaters = theaters.ToList(),
                Casts = casts.ToList(),
                SelectedGenreIds = movie.MovieGenres?.Select(mg => mg.GenreId).ToList() ?? new List<int>(),
                SelectedCastIds = movie.MovieCasts?.Select(mc => mc.CastId).ToList() ?? new List<int>(),
                SelectedTheaterIds = movie.Showtimes?.Select(s => s.TheaterId).Distinct().ToList() ?? new List<int>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(MovieFormViewModel viewModel)
        {
            // Remove PosterImage from ModelState validation since it's populated from PosterFile
            ModelState.Remove("PosterImage");

            if (ModelState.IsValid)
            {
                var movie = await _context.Movies.FindAsync(viewModel.Id);
                if (movie == null)
                {
                    return NotFound();
                }

                // Handle file upload
                if (viewModel.PosterFile != null && viewModel.PosterFile.Length > 0)
                {
                    // Create uploads directory if it doesn't exist
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Delete old file if it exists and is not the default
                    if (!string.IsNullOrEmpty(movie.PosterImage) && !movie.PosterImage.Contains("placeholder"))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", movie.PosterImage);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Generate unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.PosterFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.PosterFile.CopyToAsync(fileStream);
                    }

                    // Store only the filename in the database
                    movie.PosterImage = uniqueFileName;
                }

                movie.Title = viewModel.Title;
                movie.Description = viewModel.Description;
                movie.DurationMinutes = viewModel.DurationMinutes;
                movie.ReleaseYear = viewModel.ReleaseYear;
                movie.Rating = viewModel.Rating;
                movie.Format = viewModel.Format;
                movie.HasSubtitles = viewModel.HasSubtitles;
                movie.IsFeatured = viewModel.IsFeatured;
                movie.Price = viewModel.Price;
                movie.Language = viewModel.Language;
                movie.DirectorId = viewModel.DirectorId;
                movie.TrailerUrl = viewModel.TrailerUrl;

                // Only update PosterImage if a new file was uploaded or if it's explicitly set
                if (viewModel.PosterFile == null || viewModel.PosterFile.Length == 0)
                {
                    // Keep the existing PosterImage if no new file was uploaded
                    // This ensures we don't lose the image path when editing other fields
                }

                _context.Update(movie);

                // Update movie genres
                var existingGenres = await _context.MovieGenres
                    .Where(mg => mg.MovieId == movie.Id)
                    .ToListAsync();

                _context.MovieGenres.RemoveRange(existingGenres);

                if (viewModel.SelectedGenreIds != null)
                {
                    foreach (var genreId in viewModel.SelectedGenreIds)
                    {
                        _context.MovieGenres.Add(new MovieGenre
                        {
                            MovieId = movie.Id,
                            GenreId = genreId
                        });
                    }
                }

                // Update movie cast
                var existingCasts = await _context.MovieCasts
                    .Where(mc => mc.MovieId == movie.Id)
                    .ToListAsync();

                _context.MovieCasts.RemoveRange(existingCasts);

                if (viewModel.SelectedCastIds != null)
                {
                    foreach (var castId in viewModel.SelectedCastIds)
                    {
                        _context.MovieCasts.Add(new MovieCast
                        {
                            MovieId = movie.Id,
                            CastId = castId
                        });
                    }
                }

                // Update theaters (showtimes)
                var existingShowtimes = await _context.Showtimes
                    .Where(s => s.MovieId == movie.Id)
                    .ToListAsync();

                // Get distinct theater IDs from existing showtimes
                var existingTheaterIds = existingShowtimes.Select(s => s.TheaterId).Distinct().ToList();

                // Find theaters to add
                var theatersToAdd = viewModel.SelectedTheaterIds != null ?
                    viewModel.SelectedTheaterIds.Except(existingTheaterIds).ToList() :
                    new List<int>();

                // Add new showtimes for new theaters
                foreach (var theaterId in theatersToAdd)
                {
                    var showtime = new Showtime
                    {
                        MovieId = movie.Id,
                        TheaterId = theaterId,
                        StartTime = DateTime.Now.AddDays(1).Date.AddHours(18), // Default to tomorrow at 6 PM
                        EndTime = DateTime.Now.AddDays(1).Date.AddHours(18 + (movie.DurationMinutes / 60.0))
                    };
                    _context.Showtimes.Add(showtime);
                }

                // Find theaters to remove
                var theatersToRemove = existingTheaterIds.Except(viewModel.SelectedTheaterIds ?? new List<int>()).ToList();

                // Remove showtimes for theaters that are no longer selected
                var showtimesToRemove = existingShowtimes.Where(s => theatersToRemove.Contains(s.TheaterId)).ToList();
                _context.Showtimes.RemoveRange(showtimesToRemove);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MoviesManagement));
            }

            // If we got this far, something failed, redisplay form
            viewModel.Genres = (await _genreRepository.GetAllAsync()).ToList();
            viewModel.Directors = (await _directorRepository.GetAllAsync()).ToList();
            viewModel.Theaters = (await _theaterRepository.GetAllTheatersAsync()).ToList();
            viewModel.Casts = (await _castRepository.GetAllAsync()).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                // Delete poster file if it exists and is not the default
                if (!string.IsNullOrEmpty(movie.PosterImage) && !movie.PosterImage.Contains("placeholder"))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", movie.PosterImage);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MoviesManagement));
        }

        public async Task<IActionResult> TheatersManagement()
        {
            var theaters = await _theaterRepository.GetAllTheatersAsync();
            return View(theaters);
        }

        public async Task<IActionResult> CastAndDirectors()
        {
            var casts = await _castRepository.GetAllAsync();
            var directors = await _directorRepository.GetAllAsync();

            var viewModel = new CastAndDirectorsViewModel
            {
                Casts = casts.ToList(),
                Directors = directors.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddCast(Cast cast)
        {
            ModelState.Remove("Photo");
            ModelState.Remove("MovieCasts");
            if (ModelState.IsValid)
            {
                await _castRepository.AddAsync(cast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CastAndDirectors));
            }
            return RedirectToAction(nameof(CastAndDirectors));
        }

        [HttpPost]
        public async Task<IActionResult> AddDirector(Director director)
        {
            ModelState.Remove("ProfilePictureUrl");
            ModelState.Remove("Movies");
            if (ModelState.IsValid)
            {
                await _directorRepository.AddAsync(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CastAndDirectors));
            }
            return RedirectToAction(nameof(CastAndDirectors));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCast(int id)
        {
            var castWithid = await _context.Casts.FindAsync(id);
            if (castWithid != null)
            {
                _castRepository.Remove(castWithid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CastAndDirectors));
            }
            return RedirectToAction(nameof(CastAndDirectors));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var DirectorWithid = await _context.Directors.FindAsync(id);
            if (DirectorWithid != null)
            {
                _directorRepository.Remove(DirectorWithid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CastAndDirectors));
            }
            return RedirectToAction(nameof(CastAndDirectors));
        }

        public IActionResult AddTheater()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTheater(Theater theater)
        {
            ModelState.Remove("Capacity");
            if (ModelState.IsValid)
            {
                await _theaterRepository.AddTheaterAsync(theater);
                return RedirectToAction(nameof(TheatersManagement));
            }
            return View(theater);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTheater(int id)
        {
            await _theaterRepository.DeleteTheaterAsync(id);
            return RedirectToAction(nameof(TheatersManagement));
        }
    }
}

namespace CinemaMVC.Models.ViewModels
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }
        public List<Showtime> Showtimes { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Movie> SimilarMovies { get; set; }
        public DateTime SelectedDate { get; set; }

        public MovieDetailsViewModel()
        {
            Showtimes = new List<Showtime>();
            Genres = new List<Genre>();
            SimilarMovies = new List<Movie>();
            SelectedDate = DateTime.Today;
        }
    }
}
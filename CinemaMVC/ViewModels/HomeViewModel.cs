namespace CinemaMVC.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Movie> AllMovies { get; set; }
        public Movie FeaturedMovie { get; set; }
        public IEnumerable<Movie> PopularMovies { get; set; }
        public IEnumerable<Movie> TopRatedMovies { get; set; }
        public IEnumerable<Movie> NowShowingMovies { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

        public HomeViewModel()
        {
            AllMovies = new List<Movie>();
            PopularMovies = new List<Movie>();
            TopRatedMovies = new List<Movie>();
            NowShowingMovies = new List<Movie>();
            Genres = new List<Genre>();
        }
    }
}
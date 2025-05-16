namespace CinemaMVC.Models.ViewModels
{
    public class MoviesListViewModel
    {
        public List<Movie> Movies { get; set; }
        public List<Genre> Genres { get; set; }
        public string SelectedGenre { get; set; }
        public string SearchQuery { get; set; }
        public string SortBy { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalMovies { get; set; }

        public MoviesListViewModel()
        {
            Movies = new List<Movie>();
            Genres = new List<Genre>();
            CurrentPage = 1;
            PageSize = 8;
            SelectedGenre = "All";
            SortBy = "popularity";
        }
    }
}
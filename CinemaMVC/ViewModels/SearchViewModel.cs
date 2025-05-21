namespace CinemaMVC.Models.ViewModels
{
    public class SearchViewModel
    {
        public List<Movie> Results { get; set; } = new();
        public string SearchTerm { get; set; }
    }
}

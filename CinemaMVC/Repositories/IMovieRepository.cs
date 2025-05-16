using CinemaMVC.Models;

namespace CinemaMVC.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetTopRatedAsync(int count);
        Task<IEnumerable<Movie>> GetPopularAsync(int count);
        Task<IEnumerable<Movie>> GetAllMovieAsync();
        Task<Movie> GetMovieWithGenres(int id);
        Task<Movie> BookingMovie(int id);
    }
}

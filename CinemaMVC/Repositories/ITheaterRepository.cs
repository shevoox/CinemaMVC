using CinemaMVC.Models;

namespace CinemaMVC.Repositories
{
    public interface ITheaterRepository
    {
        Task<IEnumerable<Theater>> GetAllTheatersAsync();
        Task<Theater> GetTheaterByIdAsync(int id);
        Task<Theater> GetTheaterWithShowtimesAsync(int id);
        Task AddTheaterAsync(Theater theater);
        Task UpdateTheaterAsync(Theater theater);
        Task DeleteTheaterAsync(int id);
    }
} 
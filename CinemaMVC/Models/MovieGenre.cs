namespace CinemaMVC.Models
{
    // Join entity for the many-to-many relationship between Movie and Genre
    public class MovieGenre
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }

        // Navigation properties
        public virtual Movie Movie { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
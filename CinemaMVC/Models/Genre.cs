using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string IconClass { get; set; } // Font Awesome icon class (e.g., "fa-bolt" for Action)

        // Navigation properties
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public Genre()
        {
            MovieGenres = new HashSet<MovieGenre>();
        }
    }
}





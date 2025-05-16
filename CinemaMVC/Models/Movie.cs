using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(200)]
        public string PosterImage { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [StringLength(50)]
        public string Format { get; set; } // IMAX, 3D, Standard, 4DX, etc.

        public bool HasSubtitles { get; set; }

        [StringLength(50)]
        public string ReleaseYear { get; set; }
        public string? Director { get; set; }
        public string? Language { get; set; }
        public bool IsFeatured { get; set; }
        public int? BookedNumber { get; set; }
        // Navigation properties
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<Showtime> Showtimes { get; set; }
        public virtual ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();




    }
}
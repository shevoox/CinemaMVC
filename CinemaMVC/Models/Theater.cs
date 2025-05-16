using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models
{
    public class Theater
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [StringLength(50)]
        public string TheaterType { get; set; } // IMAX, Standard, 4DX, etc.

        // Navigation properties
        public virtual ICollection<Showtime> Showtimes { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
        public Theater()
        {
            Showtimes = new HashSet<Showtime>();
        }
    }
}
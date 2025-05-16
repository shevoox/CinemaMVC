using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaMVC.Models
{
    public class Showtime
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public int TheaterId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalSeats { get; set; }

        [NotMapped]
        public int AvailableSeats
        {
            get
            {
                int bookedSeats = Bookings?.Count ?? 0;
                return Theater != null ? Theater.Capacity - bookedSeats : 0;
            }
        }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        // Navigation properties
        public virtual Movie Movie { get; set; }
        public virtual Theater Theater { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        // Helper properties
        public string FormattedStartTime => StartTime.ToString("h:mm tt");
        public string FormattedEndTime => EndTime.ToString("h:mm tt");
        public string FormattedDate => StartTime.ToString("MMM dd");
        public bool IsSoldOut => AvailableSeats <= 0;
        public bool HasFewSeatsLeft => AvailableSeats > 0 && AvailableSeats <= 10;


    }
}
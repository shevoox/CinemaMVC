using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ShowtimeId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Range(1, int.MaxValue)]
        public int NumberOfSeats { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        public bool IsPaid { get; set; }

        [StringLength(50)]
        public string BookingStatus { get; set; } // Confirmed, Cancelled, Pending

        [StringLength(100)]
        public string PaymentMethod { get; set; }

        [StringLength(200)]
        public string TransactionId { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual Showtime Showtime { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }

        public Booking()
        {
            BookingDate = DateTime.Now;
            BookingStatus = "Pending";
            Seats = new HashSet<Seat>();
        }
    }
}
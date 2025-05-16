using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models
{
    public class Seat
    {
        public int Id { get; set; }

        public int TheaterId { get; set; }

        [Required]
        [StringLength(10)]
        public string Row { get; set; }

        [Required]
        [StringLength(10)]
        public string Number { get; set; }

        [StringLength(50)]
        public string SeatType { get; set; } // Regular, VIP, Couple, etc.

        public bool IsAvailable { get; set; }

        public int? BookingId { get; set; }

        // Navigation properties
        public virtual Theater Theater { get; set; }
        public virtual Booking Booking { get; set; }

        // Helper properties
        public string SeatPosition => $"{Row}{Number}";
    }
}
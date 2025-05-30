using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models.ViewModels
{
    public class PaymentVM
    {

        public string MovieTitle { get; set; }

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
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
    }
}

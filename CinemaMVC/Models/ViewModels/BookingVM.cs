namespace CinemaMVC.Models.ViewModels
{
    public class BookingVM
    {
        public Movie Movie { get; set; }
        public Showtime Showtime { get; set; }
        public List<Seat> Seats { get; set; }
        public Booking Booking { get; set; }
        public bool IsSoldOut { get; set; } = false;
        public int MovieId { get; set; }
        public List<string> SelectedSeats { get; set; }
        public int ShowtimeId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

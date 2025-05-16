namespace CinemaMVC.Models.ViewModels
{
    public class BookingViewModel
    {
        public Movie Movie { get; set; }
        public Showtime Showtime { get; set; }
        public Theater Theater { get; set; }
        public List<Seat> AvailableSeats { get; set; }
        public List<string> SelectedSeats { get; set; }
        public decimal TotalPrice { get; set; }
        public int NumberOfSeats => SelectedSeats?.Count ?? 0;

        public BookingViewModel()
        {
            AvailableSeats = new List<Seat>();
            SelectedSeats = new List<string>();
        }
    }
}
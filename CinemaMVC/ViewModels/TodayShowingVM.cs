namespace CinemaMVC.ViewModels
{
    public class TodayShowingVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PosterImage { get; set; }
        public int Duration { get; set; } // الدقائق
        public decimal Rating { get; set; }
        public string Theater { get; set; }
        public int AvailableSeats { get; set; }
        public string NextShowtime { get; set; }
        public string Format { get; set; } // IMAX, 3D, Standard, 4DX, etc.
    }
}


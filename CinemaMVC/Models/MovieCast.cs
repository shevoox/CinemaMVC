namespace CinemaMVC.Models
{
    public class MovieCast
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int CastId { get; set; }
        public Cast Cast { get; set; }

        // ممكن تضيف خصائص إضافية مثل دور الممثل (Character)
        public string? CharacterName { get; set; }
    }
}

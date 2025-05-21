using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models
{
    public class Director
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? ProfilePictureUrl { get; set; } = "";


        public string Bio { get; set; }

        // Navigation property
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
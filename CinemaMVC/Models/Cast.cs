using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models
{
    public class Cast
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Photo { get; set; } // صورة الممثل

        // Navigation property
        public virtual ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();
    }
}

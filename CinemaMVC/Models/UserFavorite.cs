using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CinemaMVC.Models
{
    public class UserFavorite
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public int MovieId { get; set; }
        
        public DateTime DateAdded { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
} 
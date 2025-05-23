using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string ProfileImage { get; set; }

        // Navigation properties
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<UserFavorite> UserFavorites { get; set; }

        // Helper properties
        public string FullName => $"{FirstName} {LastName}";

        public ApplicationUser()
        {
            Bookings = new HashSet<Booking>();
            UserFavorites = new HashSet<UserFavorite>();
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.ViewModels
{
    public class RegisterVM
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; } = null!;
        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageString { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}

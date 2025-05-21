using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserNameorEmail { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RemmberMe { get; set; }

    }
}

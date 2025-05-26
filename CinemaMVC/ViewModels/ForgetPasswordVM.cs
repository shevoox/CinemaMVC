using System.ComponentModel.DataAnnotations;

namespace CinemaMVC.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string UserNameOrEmail { get; set; } = null!;
    }
}

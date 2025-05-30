using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaMVC.Models.ViewModels
{
    public class UserWithRoles
    {
        public ApplicationUser applicationUser { get; set; }
        public string SelectedRole { get; set; }
        public List<SelectListItem> RoleItems { get; set; }
    }
}

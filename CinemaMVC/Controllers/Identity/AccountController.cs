using CinemaMVC.Models;
using CinemaMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaMVC.Controllers.Identity
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var vm = new AccountCombinedVM
            {
                RegisterVM = new RegisterVM(),
                LoginVM = new LoginVM()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountCombinedVM accountVM)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var registerVM = accountVM.RegisterVM;
            ModelState.Remove("LoginVM.UserNameorEmail");
            ModelState.Remove("LoginVM.Password");
            ModelState.Remove("LoginVM.RemmberMe");

            ModelState.Remove("ProfileImage");
            if (!ModelState.IsValid)
            {
                return View(accountVM);
            }

            string userImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "user");
            string filePath = Path.Combine(userImagePath, "user_image.png");

            ApplicationUser applicationUser = new()
            {
                UserName = registerVM.UserName,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                ProfileImage = registerVM.ProfileImageString ?? filePath
            };

            var result = await _userManager.CreateAsync(applicationUser, registerVM.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                TempData["Notification"] = "Account registered successfully!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }

            return View(accountVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountCombinedVM accountVM)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.Remove("RegisterVM.FirstName");
            ModelState.Remove("RegisterVM.LastName");
            ModelState.Remove("RegisterVM.UserName");
            ModelState.Remove("RegisterVM.Email");
            ModelState.Remove("RegisterVM.Password");
            ModelState.Remove("RegisterVM.ConfirmPassword");

            var loginVM = accountVM.LoginVM;

            if (!ModelState.IsValid)
            {
                return View("Register", accountVM);
            }

            var user = await _userManager.FindByNameAsync(loginVM.UserNameorEmail)
                       ?? await _userManager.FindByEmailAsync(loginVM.UserNameorEmail);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RemmberMe, false);
                if (result.Succeeded)
                {
                    TempData["Notification"] = "Welcome back!";
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View("Register", accountVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Notification"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ApplicationUser model, IFormFile ProfileImage)
        {
            if (!ModelState.IsValid)
            {
                return View("Settings", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            // Handle profile image upload
            if (ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "user");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                user.ProfileImage = "/images/user/" + uniqueFileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Notification"] = "Profile updated successfully!";
                return RedirectToAction("Settings");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Settings", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            if (string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
            {
                TempData["Error"] = "All fields are required.";
                return RedirectToAction("Settings");
            }

            if (NewPassword != ConfirmPassword)
            {
                TempData["Error"] = "New password and confirmation password do not match.";
                return RedirectToAction("Settings");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Notification"] = "Password changed successfully!";
                return RedirectToAction("Settings");
            }

            foreach (var error in result.Errors)
            {
                TempData["Error"] = error.Description;
            }

            return RedirectToAction("Settings");
        }
    }
}

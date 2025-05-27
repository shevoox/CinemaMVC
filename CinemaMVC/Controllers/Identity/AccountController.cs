using CinemaMVC.Models;
using CinemaMVC.Repositories;
using CinemaMVC.Utility;
using CinemaMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CinemaMVC.Controllers.Identity
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMovieRepository _movieRepository;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IWebHostEnvironment webHostEnvironment,
                                 IMovieRepository movieRepository, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _movieRepository = movieRepository;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new(SD.Admin));
                await _roleManager.CreateAsync(new(SD.User));
            }
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
                await _userManager.AddToRoleAsync(applicationUser, SD.User);
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
            #region createAdmin
            // Check for admin login
            //if (loginVM.UserNameorEmail.ToLower() == "admin" && loginVM.Password == "admin")
            //{
            //    var adminUser = await _userManager.FindByNameAsync("admin");
            //    if (adminUser != null)
            //    {
            //        // Delete existing admin user
            //        await _userManager.DeleteAsync(adminUser);
            //    }

            //    // Create new admin user
            //    adminUser = new ApplicationUser
            //    {
            //        UserName = "admin",
            //        Email = "admin@admin.com",
            //        FirstName = "Admin",
            //        LastName = "User",
            //        ProfileImage = "/images/user/user_image.png"
            //    };

            //    var result = await _userManager.CreateAsync(adminUser, "admin");
            //    if (!result.Succeeded)
            //    {
            //        foreach (var error in result.Errors)
            //        {
            //            ModelState.AddModelError(string.Empty, $"Admin creation error: {error.Description}");
            //        }
            //        return View("Register", accountVM);
            //    }
            //}
            #endregion
            var user = await _userManager.FindByNameAsync(loginVM.UserNameorEmail)
                       ?? await _userManager.FindByEmailAsync(loginVM.UserNameorEmail);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RemmberMe, false);

                if (result.Succeeded)
                {
                    TempData["Notification"] = "Welcome back!";

                    // Check if user is in Admin role
                    if (await _userManager.IsInRoleAsync(user, SD.Admin))
                    {
                        return RedirectToAction("Admin", "Admin");
                    }

                    // Otherwise, normal user
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

            // Get user's favorite movies
            var favoriteMovies = await _movieRepository.GetUserFavoriteMoviesAsync(user.Id);
            ViewBag.FavoriteMovies = favoriteMovies;

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
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(forgetPasswordVM);
            }
            var user = await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail)
                      ?? await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail);
            if (user == null)
            {
                return View("ForgetPasswordConfirmation");
            }
            if (user != null)
            {
                TempData["_ValidationToken"] = Guid.NewGuid().ToString();
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var confirmationLink = Url.Action("ResetPassword", "Account",
                    new { email = forgetPasswordVM.UserNameOrEmail, token = token }, Request.Scheme);
                await _emailSender.SendEmailAsync(
     email: forgetPasswordVM.UserNameOrEmail,
     subject: "Reset your password",
     htmlMessage: $"Click here to reset your password: <a href='{confirmationLink}'>Reset Password</a>"
 );

                return View("ForgetPasswordConfirmation");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View("Register", forgetPasswordVM);

        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (TempData["_ValidationToken"] is not null)
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                    return BadRequest("Invalid password reset request.");

                var model = new ResetPasswordVM
                {
                    Email = email,
                    Token = token
                };
                return View(model);
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            ModelState.Remove("Email");
            ModelState.Remove("Token");
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return View("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
                return View("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

    }
}

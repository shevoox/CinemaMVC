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
                TempData["Notifacation"] = "Register Account Successfully";
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
                    TempData["Notifacation"] = "Login Successfully";
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View("Register", accountVM);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

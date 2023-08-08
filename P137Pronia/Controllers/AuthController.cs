using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P137Pronia.Models;
using P137Pronia.ViewModels.AuthVMs;

namespace P137Pronia.Controllers
{
    public class AuthController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                Fullname = vm.Name + " " + vm.Surname,
                Email = vm.Email, 
                UserName = vm.Username
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username, email or password is wrong");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user,vm.Password,vm.RememberMe,true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Wait untill " + user.LockoutEnd.Value.AddHours(4).ToString("HH:mm:ss"));
                return View();
            }
            if (!result.Succeeded)
            {
                
                ModelState.AddModelError("", "Username, email or password is wrong");

                return View();
            }
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}

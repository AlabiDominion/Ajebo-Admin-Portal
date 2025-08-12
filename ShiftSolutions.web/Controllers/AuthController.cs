using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShiftSolutions.web.Data;
using System.ComponentModel.DataAnnotations;

namespace ShiftSolutions.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly UserManager<ApplicationUser> _users;

        public AuthController(SignInManager<ApplicationUser> signIn, UserManager<ApplicationUser> users)
        {
            _signIn = signIn;
            _users = users;
        }

        public class LoginVM
        {
            [Required, EmailAddress] public string Email { get; set; } = "";
            [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
            public bool RememberMe { get; set; }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM m, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(m);

            var user = await _users.FindByEmailAsync(m.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View(m);
            }

            var result = await _signIn.PasswordSignInAsync(user, m.Password, m.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
                return Redirect(returnUrl ?? Url.Action("Index", "Home")!);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account locked. Try again later.");
                return View(m);
            }

            ModelState.AddModelError("", "Invalid credentials.");
            return View(m);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Denied() => Content("Access denied.");
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth02_AspNetIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                // Sign in user.
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
                LockoutEnabled = false,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Generation of the email token
                await SendVerificationEmailAsync(user);

                // Sign in user here.
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        private async Task SendVerificationEmailAsync(IdentityUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());

            await _emailService.SendAsync(user.Email, "Email Verification", $"<a href=\"{link}\">Verify email</a>", true);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest("OOPS, Somethings went wrong!");

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Auth01_CookieAuth.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return Content("Here is secret page");
        }

        public IActionResult Authenticate()
        {
            var mohammadClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Mohammad"),
                new Claim(ClaimTypes.Email, "webdev.ahmadi@gmail.com"),
            };

            var licenceClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Mohammad Ahmadi"),
                new Claim("DrivingLicence", "A+"),
            };

            var mohammadIdentity = new ClaimsIdentity(mohammadClaims, "Mohammad Identity");
            var licenceIdentity = new ClaimsIdentity(licenceClaim, "Government");

            var userPrinciple = new ClaimsPrincipal(new[] { mohammadIdentity, licenceIdentity });

            HttpContext.SignInAsync(userPrinciple);

            return RedirectToAction(nameof(Index));
        }
    }
}

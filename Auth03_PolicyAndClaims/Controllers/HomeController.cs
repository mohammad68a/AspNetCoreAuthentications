using Auth03_PolicyAndClaims.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Auth03_PolicyAndClaims.Controllers
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

        [Authorize(Policy = PolicyNames.MobileRequired)]
        public IActionResult SecretPolicy()
        {
            return Content("Here is secret page");
        }

        [Authorize(Roles = RoleNames.Admin)]
        public IActionResult SecretRole()
        {
            return Content("Here is secret page");
        }

        public IActionResult Authenticate()
        {
            var mohammadClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Mohammad"),
                new Claim(ClaimTypes.Role, RoleNames.Admin),
                new Claim(ClaimTypes.MobilePhone, "+989179995199"),
                new Claim(ClaimTypes.Email, "webdev.ahmadi@gmail.com"),
            };

            var mohammadIdentity = new ClaimsIdentity(mohammadClaims, "Mohammad Identity");

            var userPrinciple = new ClaimsPrincipal(new[] { mohammadIdentity });

            HttpContext.SignInAsync(userPrinciple);

            return RedirectToAction(nameof(Index));
        }
    }
}

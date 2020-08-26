using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Auth04_RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "*")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var mohammadClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.Email, $"{Username}@sample.com"),
            };

            var mohammadIdentity = new ClaimsIdentity(mohammadClaims, "Identity");

            var userPrinciple = new ClaimsPrincipal(new[] { mohammadIdentity });

            HttpContext.SignInAsync(userPrinciple);

            return RedirectToPage("/index");
        }
    }
}

﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth05_JwtTokenServer.Controllers
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
            return Ok("Here is secret page!");
        }

        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signInCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer, 
                Constants.Audience, 
                claims,
                notBefore: DateTime.Now, 
                expires: DateTime.Now.AddDays(1), 
                signInCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenJson });
        }

        public IActionResult Decode(string part)
        {
            var bytes = Convert.FromBase64String(part);
            return Ok(Encoding.UTF8.GetString(bytes));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.DbModels;
using InvoiceApplication.Services;
using InvoiceApplication.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace InvoiceApplication.Controllers
{
    public class LoginController : Controller
    {
        private TrackingDbContext _context;
        private IUserService _userService;

        public LoginController(TrackingDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        // GET: /<controller>/
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignIn model)
        {
            if (ModelState.IsValid)
            {
                Btsuser user;
                if (await _userService.ValidateCredentials(model.Username, model.Password, out user))
                {
                    await SignInUser(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public async Task SignInUser(Btsuser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim("UserName", user.UserName.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", null);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }
    }
}
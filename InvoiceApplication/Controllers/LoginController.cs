using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Services;
using InvoiceApplication.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Web;
using InvoiceApplication.DataAccessLayer;

namespace InvoiceApplication.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _userService;

        public LoginController( IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult SignIn(String returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                if (User.Identity.IsAuthenticated)
                {
                    return Redirect(returnUrl);
                }
                ViewBag.ReturnUrl = returnUrl;
            }
            return View();
        }
        // GET: /<controller>/
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignIn model, string returnUrl=null)
        {

            InvoiceSearchViewModel searchModel = new InvoiceSearchViewModel();
            searchModel.Results = 0;
            searchModel.Status = "pending";
            searchModel.From = string.Empty;
            searchModel.To = string.Empty;

            if (ModelState.IsValid)
            {
                DbUserModel user;
                if (await _userService.ValidateCredentials(model.Username, model.Password, out user))
                {
                    await SignInUser(user);
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                   
                    if (user.CategoryId == 1)
                    {
                       
                        return RedirectToAction("ViewInvoices", "CheckingAuthority",searchModel);

                    }
                    else if(user.CategoryId == 2)
                    {
                        return RedirectToAction("ViewInvoices", "ApproverAuthority",searchModel);

                    }
                }
                var roles = User.FindFirst(ClaimTypes.Role).Value;
                if(roles == "1")
                {
                    return RedirectToAction("ViewInvoices", "CheckingAuthority", searchModel);

                }
                else if(roles == "2")
                {
                    return RedirectToAction("ViewInvoices", "ApproverAuthority", searchModel);

                }
            }
            return View(model);
        }

        public async Task SignInUser(DbUserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName.ToString()),
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(ClaimTypes.Role,user.CategoryId.ToString())
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
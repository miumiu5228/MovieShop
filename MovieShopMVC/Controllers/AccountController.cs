using ApplicationCore.Models;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieShopMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var createUser = await _userService.RegisterUser(model);

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }



        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userService.Login(model.Email, model.Password);

            if (user == null)
            {
                // wrong password
                ModelState.AddModelError(string.Empty, "Invalid password");
                return View();
            }

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.GivenName, user.FirstName),
                 new Claim(ClaimTypes.Surname, user.LastName),
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // identity object

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // create the cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect("~/");



        }
    }
}

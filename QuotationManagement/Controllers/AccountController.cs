using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuotationManagement.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            var googleId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (googleId != null)
            {
                return RedirectToAction("Search", "Quotation");
            }

            return View("/Views/Account/Login.cshtml");
        }

        [AllowAnonymous]
        public IActionResult SSOLogin(string returnURL = "/")
        {
            string decodedURL = Uri.UnescapeDataString(returnURL);
            if (decodedURL == "/account/profile")
            {
                decodedURL = "/";
            }

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = decodedURL
            }, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/account/login");
        }
    }
}

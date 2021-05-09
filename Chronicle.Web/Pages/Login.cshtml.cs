using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chronicle.Web.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        IUserStore _userStore;

        public LoginModel(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string paramUsername, string paramPassword)
        {
            string returnUrl = Url.Content("~/");

            try
            {
                // Clear the existing external cookie
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }

            if (!_userStore.HasUser(paramUsername))
            {
                return LocalRedirect(returnUrl);
            }

            var user = _userStore.GetUser(paramUsername);
            if (!user.Password.Equals(paramPassword))
            {
                return LocalRedirect(returnUrl);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, paramUsername),
                new Claim(ClaimTypes.Role, "Standard"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };

            try
            {
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return LocalRedirect(returnUrl);
        }
    }
}

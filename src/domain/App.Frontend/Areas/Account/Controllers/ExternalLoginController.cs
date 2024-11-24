using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Frontend.Areas.Account.Models;

namespace App.Frontend.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("{controller}/{action}")]
    [AllowAnonymous]
    public class ExternalLoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ExternalLoginController> _logger;
        private readonly IUserStore<User> _userStore;
        [ActivatorUtilitiesConstructor]
        public ExternalLoginController(
                                        UserManager<User> userManager,
                                        SignInManager<User> signInManager,
                                        IUserStore<User> userStore,
                                        ILogger<ExternalLoginController> logger
                                        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginWithProvider(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLogin), "ExternalLogin", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                _logger.LogError($"Error from external provider: {remoteError}");
                return RedirectToAction("Login", "Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Authentication", "Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Authentication", "AccessDenied");
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.HasClaim(c => c.Type == ClaimTypes.Email) ? info.Principal.FindFirstValue(ClaimTypes.Email) : null;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmation { Email = email });
            }
        }
    }
}

using App.Frontend.Areas.Account.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[action]")]
    [AllowAnonymous]
    [Authorize]
    public class LoginController : Controller
    {
        private readonly IAuthAPIService _authAPIService;

        public LoginController(IAuthAPIService authAPIService)
        {
            _authAPIService = authAPIService;
        }

        // GET: /Account/Login
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            // var model = new LoginViewModel
            // {
            //     ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            // };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest obj)
        {
            //     if (!ModelState.IsValid)
            //     {
            //         return RedirectToAction("Login");
            //     }

            //     string returnUrl = Url.Content("~/Customer/Index");
            // model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Require the user to have a confirmed email before they can log on.
            // var user = await _userManager.FindByEmailAsync(model.Email);
            // if (user != null)
            // {
            //     if (!await _userManager.IsEmailConfirmedAsync(user))
            //     {
            //         string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //         var callbackUrl = Url.Action("ConfirmEmail", "Account",
            //            new { userId = user.Id, code = code }, protocol: Request.Scheme);
            //         await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            //                 $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            //         ViewBag.errorMessage = "You must have a confirmed email to log on. "
            //                             + "The confirmation token has been resent to your email account.";
            //         return View("Error");
            //     }
            // }

            // var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            // if (result.Succeeded)
            // {
            //     return LocalRedirect(returnUrl);
            // }
            // if (result.IsLockedOut)
            // {
            //     return RedirectToAction("Lockout");
            // }
            // else
            // {
            //     ModelState.AddModelError(string.Empty, "Invalid Login Information.");
            //     model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //     return View(model);
            // }
            return View();
        }
    }

}
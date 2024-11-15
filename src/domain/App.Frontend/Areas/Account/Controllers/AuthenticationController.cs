<<<<<<<< HEAD:src/domain/App.Frontend/Areas/Account/Controllers/LoginController.cs
using System.Diagnostics;
using App.Frontend.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using App.Frontend.Services.IServices;
========
using Newtonsoft.Json;
using App.Frontend.Models;
using System.Diagnostics;
using App.Frontend.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Frontend/Areas/Account/Controllers/AuthenticationController.cs
using System.IdentityModel.Tokens.Jwt;
using App.Frontend.Areas.Account.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
<<<<<<<< HEAD:src/domain/App.Frontend/Areas/Account/Controllers/LoginController.cs
using Newtonsoft.Json;
========
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Frontend/Areas/Account/Controllers/AuthenticationController.cs

namespace App.Frontend.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("{area}/{controller}/{action}")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
<<<<<<<< HEAD:src/domain/App.Frontend/Areas/Account/Controllers/LoginController.cs
        public LoginController(
                                IAuthService authService,
                                ITokenProvider tokenProvider
========
        private readonly IConfiguration _configuration;
        private readonly string ProtectedAdminUrl;
        private readonly string ProtectedCustomerUrl;
        public AuthenticationController(
                                IAuthService authService,
                                ITokenProvider tokenProvider,
                                IConfiguration configuration
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Frontend/Areas/Account/Controllers/AuthenticationController.cs
                            )
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
            _configuration = configuration;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult> Login()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();
            if (authenticateResult.Succeeded)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(new LoginRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            _tokenProvider.ClearToken();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response response = await _authService.LoginAsync(model);
            if (response.IsSuccess && response != null)
            {
                var token = JsonConvert.DeserializeObject<Token>(Convert.ToString(response.Result));
                if (token != null)
                {
                    await SignInUser(token.AccessToken);
                    _tokenProvider.SetToken(token);
                    var roles = User.FindFirst(ClaimTypes.Role)?.Value;
                    if (roles != null)
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    return RedirectToAction("AccessDenied", "Authentication");
                }
                return RedirectToAction("Error", new AccountErrorModel { Message = "Login Bug" });
            }
            else
            {
                TempData["error"] = response.Message;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
                var token = _tokenProvider.GetToken();
                await _authService.LogoutAsync(token);
                _tokenProvider.ClearToken();
                return Redirect("~/");
            }
            catch
            {
                return RedirectToAction("Error", new AccountErrorModel { Message = "Unknow" });
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // 20 minutes = 1200;
        [ResponseCache(Duration = 1200, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Error(AccountErrorModel errorModel)
        {
            return View(new AccountErrorModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = errorModel.Message.ToString()
            });
        }
<<<<<<<< HEAD:src/domain/App.Frontend/Areas/Account/Controllers/LoginController.cs

        [HttpGet]
        public async Task<ActionResult> Login()
        {
            var token = _tokenProvider.GetToken();
            if (token != null)
            {
                await SignInUser(token.AccessToken);
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginRequest() { };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            Response response = await _authService.LoginAsync(model);
            if (response.IsSuccess && response != null)
            {
                var token = JsonConvert.DeserializeObject<Token>(Convert.ToString(response.Result));
                if (token != null)
                {
                    await SignInUser(token.AccessToken);
                    _tokenProvider.SetToken(token);
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Error", new AccountErrorModel { Message = "Login Bug" });
            }
            else
            {
                TempData["error"] = response.Message;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            var token = _tokenProvider.GetToken();
            await _authService.LogoutAsync(token);
            _tokenProvider.ClearToken();
            return RedirectToAction("Login", "Login", new { area = "Account" });
        }

========
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Frontend/Areas/Account/Controllers/AuthenticationController.cs
        private async Task SignInUser(string AccessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(AccessToken);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
               jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
            // for render information
            identity.AddClaim(new Claim(ClaimTypes.Name,
            jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties
            {
                IsPersistent = true, // Cookie sẽ tồn tại qua nhiều phiên duyệt web
            });
            HttpContext.User = principal; // Optionally, update HttpContext.User to reflect the new principal immediately in the current request
        }
    }
}
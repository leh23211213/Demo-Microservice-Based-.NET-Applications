<<<<<<< HEAD
using System.Diagnostics;
using App.Domain.Admin.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using App.Domain.Admin.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using App.Domain.Admin.Areas.Account.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
=======
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using App.Domain.Admin.Models;
using App.Domain.Admin.Utility;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using App.Domain.Admin.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using App.Domain.Admin.Areas.Account.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0

namespace App.Domain.Admin.Areas.Account.Controllers
{
    [Area("Account")]
<<<<<<< HEAD
    [Route("[Area]/[action]")]
    [AllowAnonymous]
=======
    [Route("user/{controller}")]
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
<<<<<<< HEAD
        public LoginController(
                                IAuthService authService,
                                ITokenProvider tokenProvider
=======
        private readonly IConfiguration _configuration;
        private readonly string ProtectedAdminUrl;
        private readonly string ProtectedCustomerUrl;
        public LoginController(
                                IAuthService authService,
                                ITokenProvider tokenProvider,
                                 IConfiguration configuration
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
                            )
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
<<<<<<< HEAD
=======
            _configuration = configuration;

            ProtectedAdminUrl = _configuration.GetValue<string>("ServiceUrls:ProtectedAdminUrl");
            ProtectedCustomerUrl = _configuration.GetValue<string>("ServiceUrls:ProtectedCustomerUrl");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            _tokenProvider.ClearToken();
            await HttpContext.SignOutAsync("Cookies");

            Response response = await _authService.LoginAsync(model);
            if (response.IsSuccess && response != null)
            {
                var token = JsonConvert.DeserializeObject<Token>(Convert.ToString(response.Result));
                if (token != null)
                {
                    await SignInUser(token.AccessToken);
                    var roles = User.FindFirst(ClaimTypes.Role)?.Value;
                    if (roles == StaticDetail.RoleAdmin)
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    if (roles == StaticDetail.RoleCustomer)
                    {
                        return Redirect(ProtectedCustomerUrl);
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
                await HttpContext.SignOutAsync("Cookies");
                SignOut("Cookies", "OpenIdConnect");
                var token = _tokenProvider.GetToken();
                await _authService.LogoutAsync(token);
                _tokenProvider.ClearToken();
                return Redirect("~/");
            }
            catch
            {
                return RedirectToAction("Error", new AccountErrorModel { Message = "Unknow" });
            }
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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
<<<<<<< HEAD

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

=======
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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
<<<<<<< HEAD
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
=======
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties
            {
                IsPersistent = true, // Cookie sẽ tồn tại qua nhiều phiên duyệt web
            });
            HttpContext.User = principal;// Optionally, update HttpContext.User to reflect the new principal immediately in the current request

>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
        }
    }
}
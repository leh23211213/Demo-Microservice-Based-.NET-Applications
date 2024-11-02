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

namespace App.Domain.Admin.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[action]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;
        private readonly string ProtectedAdminUrl;
        private readonly string ProtectedCustomerUrl;
        public LoginController(
                                IAuthService authService,
                                ITokenProvider tokenProvider,
                                 IConfiguration configuration
                            )
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
            _configuration = configuration;

            ProtectedAdminUrl = _configuration.GetValue<string>("ServiceUrls:ProtectedAdminUrl");
            ProtectedCustomerUrl = _configuration.GetValue<string>("ServiceUrls:ProtectedCustomerUrl");
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

        [HttpGet]
        public async Task<ActionResult> Login()
        {
            Response response = await _authService.LoginAsync();
            if (response.IsSuccess && response != null)
            {
                var token = JsonConvert.DeserializeObject<Token>(Convert.ToString(response.Result));
                if (token != null)
                {
                    await SignInUser(token.AccessToken);
                    var roles = User.FindFirst(ClaimTypes.Role)?.Value;
                    // redirect to admin domain
                    if (roles == StaticDetail.RoleAdmin)
                    {
                        return Redirect(ProtectedAdminUrl);
                    }
                    // redirect to customer domain
                    if (roles == StaticDetail.RoleCustomer)
                    {
                        return Redirect(ProtectedCustomerUrl);
                    }
                    return RedirectToAction("AccessDenied", "Account");
                }
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
                    var roles = User.FindFirst(ClaimTypes.Role)?.Value;
                    // redirect to admin domain
                    if (roles == StaticDetail.RoleAdmin)
                    {
                        return Redirect(ProtectedAdminUrl);
                    }
                    // redirect to customer domain
                    if (roles == StaticDetail.RoleCustomer)
                    {
                        return Redirect(ProtectedCustomerUrl);
                    }
                    return RedirectToAction("AccessDenied", "Account");
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
                await HttpContext.SignOutAsync();
                var token = _tokenProvider.GetToken();
                await _authService.LogoutAsync(token);
                _tokenProvider.ClearToken();
                return RedirectToAction("Login", "Login", new { area = "Account" });
            }
            catch
            {
                return RedirectToAction("Error", new AccountErrorModel { Message = "Unknow" });
            }
        }

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
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
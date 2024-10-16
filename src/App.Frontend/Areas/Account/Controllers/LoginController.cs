using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Frontend.Areas.Account.Models;
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Frontend.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[action]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public LoginController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
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
            var model = new LoginRequest()
            {
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            // Clear token before login
            var webToken = _tokenProvider.GetToken();
            if (webToken != null)
            {
                _tokenProvider.ClearToken();
            }

            Response response = await _authService.LoginAsync(model);

            if (response.IsSuccess && response != null)
            {
                var token = JsonConvert.DeserializeObject<Token>(Convert.ToString(response.Result));
                if (token != null)
                {
                    await SignInUser(token);
                    _tokenProvider.SetToken(token);
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Error", new AccountErrorModel { Message = "Token Bug" });
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

        private async Task SignInUser(Token token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token.AccessToken);

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
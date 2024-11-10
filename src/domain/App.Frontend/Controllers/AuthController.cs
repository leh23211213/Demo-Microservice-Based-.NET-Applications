using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Frontend.Controllers
{
    public class AuthController : Controller
    {

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction(nameof(Index), "Home");
            //LoginRequestDTO obj = new();
            //return View(obj);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

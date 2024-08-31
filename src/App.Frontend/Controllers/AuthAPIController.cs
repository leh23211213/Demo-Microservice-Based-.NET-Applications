
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace App.Frontend.Controllers
{
    [Route("[controller]")]
    public class AuthAPIController : Controller
    {
        private readonly IAuthAPIService _authAPIService;

        public AuthAPIController(IAuthAPIService authAPIService)
        {
            _authAPIService = authAPIService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            return View();
        }

    }
}
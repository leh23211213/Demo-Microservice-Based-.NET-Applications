using System.IdentityModel.Tokens.Jwt;
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Frontend.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Cart>> Index()
        {
            return View(LoadCartDtoBasedOnLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(Cart cart)
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        private async Task<Cart> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            Response? response = await _cartService.GetAsync(userId);
            if (response != null & response.IsSuccess)
            {
                var cart = JsonConvert.DeserializeObject<Cart>(Convert.ToString(response.Result));
                return cart;
            }
            return new Cart();
        }
    }
}
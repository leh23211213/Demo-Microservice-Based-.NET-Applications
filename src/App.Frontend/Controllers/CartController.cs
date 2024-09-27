using System.IdentityModel.Tokens.Jwt;
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;
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
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var accessToken = Request.Cookies["JWTToken"];

            Response responses = await _cartService.GetAsync(userId, accessToken);
            if (responses.Result != null && responses.IsSuccess)
            {
                Cart cart = JsonConvert.DeserializeObject<Cart>(responses.Result.ToString());
                return View(cart);
            }
            return View(new Cart());
        }



    }
}
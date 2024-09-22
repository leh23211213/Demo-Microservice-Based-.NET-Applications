using System.IdentityModel.Tokens.Jwt;
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Frontend.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ActionResult<Cart>> Index()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            Response responses = await _productService.GetAsync(userId, Request.Cookies["JWTToken"]);
            if (responses.Result != null && responses.IsSuccess)
            {
                Cart cart = JsonConvert.DeserializeObject<Cart>(responses.Result.ToString());
                return cart;
            }
            return new Cart();
        }
    }
}
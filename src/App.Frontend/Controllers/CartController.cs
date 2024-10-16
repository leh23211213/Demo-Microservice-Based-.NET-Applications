using System.IdentityModel.Tokens.Jwt;
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Frontend.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await LoadCart());
        }


        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCart());
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsync(Cart cart)
        {
            return View(await LoadCart());
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            Response response = await _orderService.ValidateStripeSession(orderId);
            if (response.IsSuccess && response != null)
            {
                return View(orderId);
            }
            return View(orderId);
        }

        public async Task<IActionResult> Delete(string cartDetailsId)
        {
            Response? response = await _cartService.DeleteAsync(cartDetailsId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<Cart> LoadCart()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            Response? response = await _cartService.GetAsync(userId);
            if (response != null && response.IsSuccess)
            {
                Cart cart = JsonConvert.DeserializeObject<Cart>(Convert.ToString(response.Result));
                return cart;
            }
            return new Cart();
        }
    }
}
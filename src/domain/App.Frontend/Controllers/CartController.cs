using Newtonsoft.Json;
using App.Frontend.Models;
using App.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(await LoadCart());
            }
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(await LoadCart());
            }
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Cart cartInfomation)
        {
            var cart = await LoadCart();
            cart.CartHeader.Phone = cartInfomation.CartHeader.Phone;

            var response = await _orderService.CreateOrder(cart);
            if (response.IsSuccess && response != null)
            {
                var orderHeader = JsonConvert.DeserializeObject<OrderHeader>(Convert.ToString(response.Result));

                //get stripe session and redirect to stripe to place order
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                StripeRequest stripeRequest = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation/orderId=" + orderHeader?.Id,
                    CancelUrl = domain + "cart/Checkout",
                    OrderHeader = orderHeader,
                };

                var stripeResponse = await _orderService.CreateStripeSession(stripeRequest);
                if (stripeResponse != null)
                {
                    var stripeResponseResult = JsonConvert.DeserializeObject<StripeRequest>(Convert.ToString(stripeResponse.Result));
                    Response.Headers?.Add("Location", stripeResponseResult.StripeSessionUrl);
                }
                return new StatusCodeResult(303);
            }
            return View(cart);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(string orderId)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response response = await _orderService.ValidateStripeSession(orderId);
                if (response.IsSuccess && response != null)
                {
                    return View(orderId);
                }
                return View(orderId);
            }
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
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
            //var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            //Hiệu suất tốt hơn: FindFirst() dừng ngay khi tìm thấy kết quả, thay vì duyệt qua tất cả các claim.
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var userEmail = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            var userName = User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;

            Response? response = await _cartService.GetAsync(userId);
            if (response != null && response.IsSuccess)
            {
                Cart cart = JsonConvert.DeserializeObject<Cart>(Convert.ToString(response.Result));
                cart.CartHeader.Name = userName;
                cart.CartHeader.Email = userEmail;
                return cart;
            }
            return new Cart();
        }
    }
}
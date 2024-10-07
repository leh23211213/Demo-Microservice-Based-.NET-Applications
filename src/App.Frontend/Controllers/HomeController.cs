using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
namespace App.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public HomeController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index([FromQuery] string? search = null, [FromQuery] int curentPage = 1)
        {
            Response? response = await _productService.Get(search, curentPage);
            Pagination pagination = new();
            if (response.IsSuccess && response != null && response.Result != null)
            {
                pagination = JsonConvert.DeserializeObject<Pagination>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(pagination);
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            Response? response = await _productService.Get(id);
            Product? product = new();
            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<Product>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(Product product)
        {
            #region CART

            Cart cart = new()
            {
                CartHeader = new CartHeader()
                {
                    UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value
                }
            };

            CartDetails cartDetails = new CartDetails()
            {
                ProductId = product.Id,
            };

            List<CartDetails> cartDetailsList = new() { cartDetails };
            cart.CartDetails = cartDetailsList;

            #endregion

            Response response = await _cartService.AddAsync(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item has been added to Cart.";
                return RedirectToAction("Details", "Home", new { Id = product.Id });
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        public async Task<IActionResult> CoverPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

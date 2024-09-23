using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using IdentityModel;
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


        public async Task<IActionResult> Index([FromBody] string? search, int curentPage = 1)
        {
            Response? response = await _productService.GetAsync(search, curentPage);
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



        public async Task<IActionResult> Detail(string id)
        {
            Response? response = await _productService.GetAsync(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product)
        {
            var userId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "User not authenticated.";
                return RedirectToAction(nameof(Index));
            }

            Cart cart = new()
            {
                CartHeader = new CartHeader()
                {
                    UserId = userId
                }
            };
            CartDetails cartDetails = new CartDetails()
            {
                ProductId = product.Id
            };

            List<CartDetails> cartDetailsList = new() { cartDetails };
            cart.CartDetails = cartDetailsList;

            var accessToken = Request.Cookies["JWTToken"];
            if (accessToken == null)
            {
                return Unauthorized("Access token is missing.");
            }

            Response response = await _cartService.AddAsync(cart, accessToken);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item has been added to Cart.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        public async Task<IActionResult> coverPage()
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

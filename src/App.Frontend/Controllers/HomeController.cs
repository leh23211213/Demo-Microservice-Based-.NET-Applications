using App.Frontend.Models;
using App.Frontend.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
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

        public async Task<IActionResult> Index(
                                                [FromQuery] int pageSize = 6,
                                                [FromQuery] int currentPage = 1,
                                                [FromQuery] string? search = ""
                                            )
        {
            Response? response = await _productService.Get(pageSize, currentPage, search);
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

        // [Authorize]
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

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(Product product)
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
                Id = Guid.NewGuid().ToString(),
                ProductId = product.Id,
            };

            List<CartDetails> cartDetailsList = new() { cartDetails };
            cart.CartDetails = cartDetailsList;

            #endregion

            Response response = await _cartService.AddAsync(cart);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction("Details", "Home", new { Id = product.Id });
        }

        public async Task<IActionResult> CoverPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

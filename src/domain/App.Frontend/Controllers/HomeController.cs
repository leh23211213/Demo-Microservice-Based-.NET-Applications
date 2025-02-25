using Newtonsoft.Json;
using System.Diagnostics;
using App.Frontend.Models;
using App.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using App.Frontend.Utility;
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
            if (User.Identity.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (User.Identity.IsAuthenticated)
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
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
        }

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
                ProductId = product.Id,
            };

            List<CartDetails> cartDetailsList = new() { cartDetails };
            cart.CartDetails = cartDetailsList;
            #endregion

            Response response = await _cartService.AddAsync(cart);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                HttpContext.Session.SetString(StaticDetail.SessionCart, JsonConvert.SerializeObject(response.Result));
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

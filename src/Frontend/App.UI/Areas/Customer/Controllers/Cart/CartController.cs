
using App.Data;
using App.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.UI.Controllers
{
    [Authorize]
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        [ActivatorUtilitiesConstructor] // BUG: MULTIple contructors add <~
        public CartController(ApplicationDbContext context, UserManager<User> userManager, CartService cartService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartViewModel = _cartService.GetCartViewModel(userId);
            if (cartViewModel == null)
            {
                return NotFound();
            }

            ViewBag.TotalTemporaryPrice = cartViewModel.TemporaryPrice;
            ViewBag.TotalPrice = cartViewModel.TotalPrice;

            return View(cartViewModel);
        }

        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }

            try
            {
                await _cartService.Add(userId, productId); // Ensure AddAsync is awaited
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // POST: Classes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }

            var cart = _cartService.GetCart(userId);
            if (cart == null)
            {
                return Json(new { success = false, message = "Cart not found" });
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("");
        }

        [HttpPost("DeleteAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }

            var cart = _context.Carts
                            .Include(c => c.CartItems)
                            .FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("");
        }

        [HttpPost("IncreaseQuantity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity(string productId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized("User not found");
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Product not found in cart");
            }

            if (cartItem.Quantity < 5)
            {
                cartItem.Quantity += 1;
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Quantity decreased successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot increase quantity below 5.";
            }

            return RedirectToAction("");
        }

        [HttpPost("DecreaseQuantity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity(string productId)
        {
            var userId = _userManager.GetUserId(User);

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Product not found in cart");
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Quantity decreased successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot decrease quantity below 1.";
            }

            return RedirectToAction("");
        }
    }
}

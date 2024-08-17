using App.Data;
using App.Data.Models;
using App.UI.Models.Cart;
using Microsoft.EntityFrameworkCore;
namespace App.UI.Controllers
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        internal Cart GetCart(string userId)
        {
            var cart = _context.Carts
               .Include(c => c.CartItems)
               .ThenInclude(ci => ci.Product)
               .FirstOrDefault(c => c.UserId == userId);

            return cart == null ? null : cart;
        }

        public CartViewModel GetCartViewModel(string userId)
        {
            var cart = GetCart(userId);

            var cartViewModel = new CartViewModel
            {
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    ImageUrl = ci.Product.ImageUrl,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    TemporaryPrice = ci.Product.Price * ci.Quantity,
                    TotalPrice = ci.Product.Price * ci.Quantity
                }).ToList()
            };

            cartViewModel.TemporaryPrice = cartViewModel.CartItems.Sum(item => item.TemporaryPrice);
            cartViewModel.TotalPrice = cartViewModel.TemporaryPrice;

            return cartViewModel;
        }

        public async Task Add(string userId, string productId, int quantity = 1)
        {
            var cart = GetCart(userId);

            if (cart.CartItems.Count >= 5)
            {
                throw new InvalidOperationException("You can only add up to 5 products to your cart.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == productId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    CartId = cart.CartId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                throw new InvalidOperationException("Product are ready exists in your cart");
            }
            await _context.SaveChangesAsync();
        }
    }
}
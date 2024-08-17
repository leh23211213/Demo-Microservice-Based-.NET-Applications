using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Data.Models;
using App.Data.Enums;
using App.UI.Models.Order;

namespace App.UI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public OrderController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        [HttpGet("")]
        public ActionResult Index()
        {
            var orders = _context.Orders.ToList();
            if (orders == null) return View();

            var orderViewModels = orders.Select(o => new OrderViewModel
            {
                UserId = o.UserId,
                OrderId = o.OrderId,
                CustomerName = o.CustomerName,
                Address = o.Address,
                City = o.City,
                Country = o.Country,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                OrderDate = o.OrderDate,
            }).ToList();

            return View(orderViewModels);
        }

        [HttpPost("Buy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart.CartItems.Count == 0)
            {
                return View();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (cart == null) throw new Exception("Cart is null.");

                    if (cart.CartItems == null || !cart.CartItems.Any()) throw new Exception("Cart items are null or empty.");

                    var order = new Order
                    {
                        UserId = userId,
                        CustomerName = "NONE",
                        Address = "NONE",
                        City = "NONE",
                        Country = "NONE",
                        TotalPrice = cart.CartItems.Sum(item => item.Quantity * item.Product.Price),
                        OrderDate = DateTime.Now
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    if (order.OrderId <= 0) throw new Exception("OrderId not generated.");

                    var orderDetails = cart.CartItems.Select(cartItem => new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Quantity * cartItem.Product.Price,
                        ProductName = cartItem.Product.ProductName,
                        ProductImage = cartItem.Product.ImageUrl,
                        Status = OrderDetailStatus.Pending
                    }).ToList();

                    _context.OrderDetails.AddRange(orderDetails);
                    _context.CartItems.RemoveRange(cart.CartItems);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return RedirectToAction("");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.ErrorMessage = "Error processing order: " + ex.Message;
                    return View("");
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> OrderDetails(int id)
        {
            var order = await _context.Orders
                        .Include(o => o.OrderDetails)
                        .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var OrderViewModel = new OrderViewModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailsViewModel
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    ProductName = od.ProductName,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    Status = od.Status,
                }).ToList()
            };
            return View(OrderViewModel);
        }
    }
}
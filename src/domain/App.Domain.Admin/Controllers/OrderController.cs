using Microsoft.AspNetCore.Mvc;
using App.Domain.Admin.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using App.Domain.Admin.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using App.Domain.Admin.Utility;

namespace App.Domain.Admin.Controllers
{
    [Authorize]
    [AllowAnonymous]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

<<<<<<< HEAD
        public IActionResult Index()
        {
            return View();
        }

=======
<<<<<<<< HEAD:src/domain/App.Frontend/Controllers/OrderController.cs
========
        [HttpGet]
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Controllers/OrderController.cs
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
        }

<<<<<<<< HEAD:src/domain/App.Frontend/Controllers/OrderController.cs
========
        [HttpGet]
>>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0:src/domain/App.Domain.Admin/Controllers/OrderController.cs
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
        public async Task<IActionResult> Details(string orderId)
        {
            OrderHeader orderHeader = new OrderHeader();
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            Response response = await _orderService.Get(orderId);
            if (response.IsSuccess && response != null)
            {
                orderHeader = JsonConvert.DeserializeObject<OrderHeader>(Convert.ToString(response.Result));
            }
            if (!User.IsInRole(StaticDetail.RoleAdmin) && userId != orderHeader.UserId)
            {
                return NotFound();
            }
            return View(orderHeader);
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(string orderId)
        {
            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(string orderId)
        {
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get(string status)
        {
            IEnumerable<OrderHeader> list;
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            Response response = _orderService.GetAllOrder(userId).GetAwaiter().GetResult();
            if (response.IsSuccess && response != null)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeader>>(Convert.ToString(response.Result));
                switch (status)
                {
                    case "approved":
                        list = list.Where(u => u.Status == StaticDetail.Status_Approved);
                        break;
                    case "readyforpickup":
                        list = list.Where(u => u.Status == StaticDetail.Status_ReadyForPickup);
                        break;
                    case "cancelled":
                        list = list.Where(u => u.Status == StaticDetail.Status_Cancelled);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                list = new List<OrderHeader>();
            }
            return Json(new { data = list.OrderByDescending(u => u.Id) });
        }
    }
}
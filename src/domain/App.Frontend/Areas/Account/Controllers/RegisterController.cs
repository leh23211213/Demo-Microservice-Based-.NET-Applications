using App.Frontend.Models;
using App.Frontend.Utility;
using App.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using App.Frontend.Areas.Account.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Frontend.Areas.Account.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IAuthService _authService;

        public RegisterController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult> Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetail.RoleAdmin,Value=StaticDetail.RoleAdmin},
                new SelectListItem{Text=StaticDetail.RoleCustomer,Value=StaticDetail.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequest model)
        {
            Response response = await _authService.RegisterAsync(model);
            Response assignRole;

            if (response.IsSuccess && response != null)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = StaticDetail.RoleCustomer;
                }
                TempData["success"] = response?.Message;

                return RedirectToAction("Login", "Authentication", new { area = "Account" });
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetail.RoleAdmin,Value=StaticDetail.RoleAdmin},
                new SelectListItem{Text=StaticDetail.RoleCustomer,Value=StaticDetail.RoleCustomer},
            };
            ViewBag.RoleList = roleList;

            return View(model);
        }
    }
}

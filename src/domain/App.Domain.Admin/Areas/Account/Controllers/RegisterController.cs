using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Domain.Admin.Services.IServices;
using App.Domain.Admin.Areas.Account.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Domain.Admin.Utility;
using App.Domain.Admin.Models;

namespace App.Domain.Admin.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("user/{controller}")]
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
                return RedirectToAction("Login", "Authentication");
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

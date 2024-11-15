<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Domain.Admin.Services.IServices;
using App.Domain.Admin.Areas.Account.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Domain.Admin.Utility;
using App.Domain.Admin.Models;
=======
using App.Domain.Admin.Models;
using App.Domain.Admin.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Domain.Admin.Services.IServices;
using App.Domain.Admin.Areas.Account.Models;
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0

namespace App.Domain.Admin.Areas.Account.Controllers
{
    [Area("Account")]
<<<<<<< HEAD
    [Route("[area]/[action]")]
    [AllowAnonymous]
=======
    [Route("user/{controller}")]
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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
<<<<<<< HEAD
            Response assignRole;
=======
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0

            if (response.IsSuccess && response != null)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = StaticDetail.RoleCustomer;
                }
                TempData["success"] = response?.Message;
<<<<<<< HEAD
                return RedirectToAction("Login", "Login", new { area = "Account" });
=======
                return RedirectToAction("Login", "Authentication", new { area = "Account" });
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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

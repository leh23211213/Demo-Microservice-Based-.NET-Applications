using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Frontend.Services.IServices;
using App.Frontend.Areas.Account.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Frontend.Utility;
using App.Frontend.Models;

namespace App.Frontend.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[area]/[action]")]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IAuthAPIService _authAPIService;

        public RegisterController(IAuthAPIService authAPIService)
        {
            _authAPIService = authAPIService;
        }

        [HttpGet]
        public async Task<ActionResult> Register()
        {
            var model = new RegistrationRequest
            {
                Roles = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.RoleAdmin, Value = StaticDetail.RoleAdmin},
                new SelectListItem{Text = StaticDetail.RoleCustomer, Value = StaticDetail.RoleCustomer}
                }
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequest model)
        {
            Response response = await _authAPIService.RegisterAsync(model);

            if (response.IsSuccess && response != null)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = StaticDetail.RoleCustomer;
                }

                Response assignRole = await _authAPIService.AssignRoleAsync(model);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction("Login", "Login", new { area = "Account" });
                }
            }
            else
            {
                TempData["error"] = response.Message;
            }

            model.Roles = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.RoleAdmin, Value = StaticDetail.RoleAdmin},
                new SelectListItem{Text = StaticDetail.RoleCustomer, Value = StaticDetail.RoleCustomer}
                };

            return View(model);
        }
    }
}

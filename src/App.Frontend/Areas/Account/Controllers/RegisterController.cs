using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using App.Frontend.Services.IServices;
using App.Frontend.Areas.Account.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Frontend.Utility;
using Microsoft.AspNetCore.Identity.Data;
using App.Frontend.Models;

namespace App.UI.Areas.Account.Controllers
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
            Response result = await _authAPIService.RegisterAsync(model);

            if (result.IsSuccess && result != null)
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
                TempData["error"] = result.Message;
            }

            model.Roles = new List<SelectListItem>(){
                new SelectListItem{Text = StaticDetail.RoleAdmin, Value = StaticDetail.RoleAdmin},
                new SelectListItem{Text = StaticDetail.RoleCustomer, Value = StaticDetail.RoleCustomer}
                };

            return View(model);
        }
    }
}

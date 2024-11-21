using IdentityModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace App.Services.AuthAPI.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
                RoleManager<IdentityRole> roleInManager,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager
              )
        {
            _signInManager = signInManager;
            _roleManager = roleInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; } = default!;
        public async Task<IActionResult> OnGet(string returnUrl)
        {
            List<string> roles = new()
            {
                StaticDetail.Admin,
                StaticDetail.Customer
            };
            ViewData["roles_message"] = roles;

            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync() ?? Enumerable.Empty<AuthenticationScheme>()).ToList(),
            };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    EmailConfirmed = true,
                    Name = Input.Name,
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(Input.RoleName).GetAwaiter().GetResult())
                    {
                        var userRole = new IdentityRole
                        {
                            Name = Input.RoleName,
                            NormalizedName = Input.RoleName,

                        };
                        await _roleManager.CreateAsync(userRole);
                    }
                    await _userManager.AddToRoleAsync(user, Input.RoleName);

                    await _userManager.AddClaimsAsync(user, new Claim[] {
                        new Claim(JwtClaimTypes.Name,Input.Email),
                        new Claim(JwtClaimTypes.Email,Input.Email),
                        new Claim(JwtClaimTypes.Role,Input.RoleName)
                    });

                    return Redirect(Input.ReturnUrl);
                }
            }
            else
            {
                List<string> roles = new()
                {
                    StaticDetail.Admin,
                    StaticDetail.Customer
                };
                ViewData["roles_message"] = roles;

                Input = new RegisterViewModel
                {
                    ReturnUrl = Input.ReturnUrl,
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync() ?? Enumerable.Empty<AuthenticationScheme>()).ToList(),
                };
            }
            return Page();
        }
    }
}
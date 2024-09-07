using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Models.DTOs;
using App.Services.AuthAPI.Service.IService;
using App.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace App.Services.AuthAPI.Services
{
    public class AuthAPIService : IAuthAPIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthAPIService(ApplicationDbContext dbContext,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var account = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (account != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(account, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var account = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == loginRequest.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(account, loginRequest.Password);

            if (account == null || !isValid)
            {
                return new LoginResponse() { User = null, Token = null };
            }

            var roles = await _userManager.GetRolesAsync(account);
            var token = _jwtTokenGenerator.GenerateToken(account, roles);

            User user = new()
            {
                Email = account.Email,
                Name = account.Name,
            };

            LoginResponse loginResponseresponse = new LoginResponse()
            {
                User = user,
                Token = token
            };

            return loginResponseresponse;
        }

        public async Task<string> Register(RegistrationRequest RegistrationRequest)
        {
            ApplicationUser applicationUser = new()
            {
                UserName = RegistrationRequest.Email,
                Email = RegistrationRequest.Email,
                NormalizedEmail = RegistrationRequest.Email.ToUpper(),
                Name = RegistrationRequest.Name,
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, RegistrationRequest.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _dbContext.Users.First(x => x.UserName == RegistrationRequest.Email);
                    User user = new()
                    {
                        Id = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                    };
                    return "";
                }
                else
                {
                    return string.Join("; ", result.Errors.Select(e => e.Description)) ?? "Unknown error";
                }
            }
            catch (Exception ex)
            {
                return $"Error Encountered: {ex.Message}";
            }
        }
    }
}
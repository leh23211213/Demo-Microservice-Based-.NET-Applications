using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
namespace App.Services.AuthAPI.Services
{
    public interface IRegisterAPIService
    {
        Task<string> Register(RegistrationRequest RegistrationRequest);
    }

    public class RegisterAPIService : IRegisterAPIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterAPIService(
                                ApplicationDbContext dbContext,
                                RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager
                            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Register(RegistrationRequest registrationRequest)
        {
            if (!IsUniqueUser(registrationRequest.Email)) return "User all ready exists";

            var id = new SnowflakeIdGenerator(datacenterId: 1, workerId: 1);

            ApplicationUser applicationUser = new()
            {
                Id = id.GenerateId().ToString(),
                UserName = registrationRequest.Email,
                Email = registrationRequest.Email,
                NormalizedEmail = registrationRequest.Email.ToUpper(),
                Name = registrationRequest.Name,
                PhoneNumber = registrationRequest.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, registrationRequest.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(registrationRequest.Role).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registrationRequest.Role));
                    }
                    await _userManager.AddToRoleAsync(applicationUser, registrationRequest.Role);
                    return string.Empty;
                }
                else
                {
                    return string.Join("; ", result.Errors.Select(e => e.Description)) ?? "Unknown error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public bool IsUniqueUser(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return true;
            }
            return false;
        }

    }
}
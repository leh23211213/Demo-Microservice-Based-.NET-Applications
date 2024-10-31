using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Services.AuthAPI.Services.IServices;

namespace App.Services.AuthAPI.Services
{
    public class AuthAPIService : IAuthAPIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthAPIService(
                                ApplicationDbContext dbContext,
                                IJwtTokenGenerator jwtTokenGenerator,
                                RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager
                            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Register(RegistrationRequest registrationRequest)
        {
            ApplicationUser applicationUser = new()
            {
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
        public async Task<Token> Login(LoginRequest loginRequest)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            /*
                if ( !user.EmailConfirmed)
                {
                    call email api and send to user for verify
                }
            */

            if (user == null || !isValid)
            {
                return new Token() { AccessToken = string.Empty };
            }

            //if user was found generate JWT Token
            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken = await _jwtTokenGenerator.CreateNewAccessToken(user, jwtTokenId);
            var refreshToken = await _jwtTokenGenerator.CreateNewRefreshToken(user.Id, jwtTokenId);

            Token token = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return token;
        }
    }
}
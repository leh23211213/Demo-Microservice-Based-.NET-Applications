using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace App.Services.AuthAPI.Services
{
    public interface ILoginAPIService
    {
        Task<Token> Login(LoginRequest loginRequest);
    }

    public class LoginAPIService : ILoginAPIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager;
        public LoginAPIService(
                                ApplicationDbContext dbContext,
                                IJwtTokenGenerator jwtTokenGenerator,
                                UserManager<ApplicationUser> userManager
                            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
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
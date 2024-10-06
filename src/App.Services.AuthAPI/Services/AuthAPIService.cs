using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.AuthAPI.Services
{
    public class AuthAPIService : IAuthAPIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;

        public AuthAPIService(ApplicationDbContext dbContext,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
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


                    var userToReturn = _dbContext.Users.First(x => x.UserName == registrationRequest.Email);
                    User user = new()
                    {
                        Id = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return string.Empty;
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
        public async Task<Token> Login(LoginRequest loginRequest)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == loginRequest.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (user == null || !isValid)
            {
                return new Token() { AccessToken = string.Empty };
            }
            //if user was found generate JWT Token
            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken = await GetAccessToken(user, jwtTokenId);
            var refreshToken = await CreateNewRefreshToken(user.Id, jwtTokenId);

            Token token = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return token;
        }
        public async Task<Token> RefreshAccessToken(Token token)
        {
            // Find an existing refresh token
            var existingRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(u => u.Refresh_Token == token.RefreshToken);
            if (existingRefreshToken == null)
            {
                return new Token();
            }

            // Compare data from existing refresh and access token provided and if there is any missmatch then consider it as a fraud
            var isTokenValid = GetAccessTokenData(token.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new Token();
            }

            // When someone tries to use not valid refresh token, fraud possible
            if (!existingRefreshToken.IsValid)
            {
                await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            }
            // If just expired then mark as invalid and return empty
            if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new Token();
            }

            // replace old refresh with a new one with updated expire date
            var newRefreshToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            // revoke existing refresh token
            await MarkTokenAsInvalid(existingRefreshToken);

            // generate new access token
            var applicationUser = _dbContext.Users.FirstOrDefault(u => u.Id == existingRefreshToken.UserId);
            if (applicationUser == null)
                return new Token();

            var newAccessToken = await GetAccessToken(applicationUser, existingRefreshToken.JwtTokenId);

            return new Token()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };

        }
        public async Task RevokeRefreshToken(Token token)
        {
            var existingRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(_ => _.Refresh_Token == token.RefreshToken);

            if (existingRefreshToken == null)
                return;
            // Compare data from existing refresh and access token provided and
            // if there is any missmatch then we should do nothing with refresh token
            var isTokenValid = GetAccessTokenData(token.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {

                return;
            }

            await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

        }
        private async Task<string> GetAccessToken(ApplicationUser user, string jwtTokenId)
        {

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Issuer",
                Audience = "Audience",
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
        private async Task<string> CreateNewRefreshToken(string user, string jwtTokenId)
        {
            RefreshToken refreshToken = new()
            {
                IsValid = true,
                UserId = user,
                JwtTokenId = jwtTokenId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(2),
                Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid(),
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }
        private Task MarkTokenAsInvalid(RefreshToken refreshToken)
        {
            refreshToken.IsValid = false;
            return _dbContext.SaveChangesAsync();
        }
        private async Task MarkAllTokenInChainAsInvalid(string userId, string tokenId)
        {
            await _dbContext.RefreshTokens.Where(u => u.UserId == userId
               && u.JwtTokenId == tokenId)
                   .ExecuteUpdateAsync(u => u.SetProperty(refreshToken => refreshToken.IsValid, false));
        }
        private bool GetAccessTokenData(string accessToken, string expectedUserId, string expectedTokenId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(accessToken);
                var jwtTokenId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti).Value;
                var userId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value;
                return userId == expectedUserId && jwtTokenId == expectedTokenId;
            }
            catch
            {
                return false;
            }
        }
    }
}
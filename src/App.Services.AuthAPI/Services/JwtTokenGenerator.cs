using System.Text;
using System.Security.Claims;
using App.Services.AuthAPI.Data;
using App.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace App.Services.AuthAPI.Services
{
    public interface IJwtTokenGenerator
    {
        Task RevokeRefreshToken(Token token);
        Task<Token> RefreshAccessToken(Token token);
        Task<string> CreateNewRefreshToken(string userId, string jwtTokenId);
        Task<string> CreateNewAccessToken(ApplicationUser user, string jwtTokenId);
    }

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string secretKey;
        private readonly string issuer;
        private readonly string audience;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public JwtTokenGenerator(
                                IConfiguration configuration,
                                ApplicationDbContext dbContext,
                                UserManager<ApplicationUser> userManager
                                )
        {
            _dbContext = dbContext;
            _userManager = userManager;

            _configuration = configuration;

            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
            issuer = _configuration.GetValue<string>("ApiSettings:Issuer");
            audience = _configuration.GetValue<string>("ApiSettings:Audience");
        }

        public async Task<string> CreateNewAccessToken(ApplicationUser user, string jwtTokenId)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Aud, audience),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
            };
            //claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer, // authen server url
                Audience = audience, // domain server url
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> CreateNewRefreshToken(string userId, string jwtTokenId)
        {
            RefreshToken refreshToken = new()
            {
                IsValid = true,
                UserId = userId,
                JwtTokenId = jwtTokenId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid(),
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            return refreshToken.Refresh_Token;
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
            var isTokenValid = GetAccessTokenData(token.AccessToken ?? "null", existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
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

            var newAccessToken = await CreateNewAccessToken(applicationUser, existingRefreshToken.JwtTokenId);

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
            var isTokenValid = GetAccessTokenData(token.AccessToken ?? "null", existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {
                return;
            }
            await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
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

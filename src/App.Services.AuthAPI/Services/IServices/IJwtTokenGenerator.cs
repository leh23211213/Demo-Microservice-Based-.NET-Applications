using App.Services.AuthAPI.Models;
namespace App.Services.AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        Task RevokeRefreshToken(Token token);
        Task<Token> RefreshAccessToken(Token token);
        Task<string> CreateNewRefreshToken(string userId, string jwtTokenId);
        Task<string> CreateNewAccessToken(ApplicationUser user, string jwtTokenId);
    }
}
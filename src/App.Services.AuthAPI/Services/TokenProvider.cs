using App.Services.AuthAPI.Models;
using App.Services.AuthAPI.Utility;
using App.Services.AuthAPI.Services.IServices;
namespace App.Services.AuthAPI.Services
{
    /// <summary>
    ///  working with cookies
    /// </summary>
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetail.AccessToken);
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetail.RefreshToken);
        }
        public Token GetToken()
        {
            try
            {
                bool hasAccessToken = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(StaticDetail.AccessToken, out string accessToken);
                bool hasRefreshToken = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(StaticDetail.RefreshToken, out string refreshToken);

                Token token = new()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
                return hasAccessToken is true ? token : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void SetToken(Token token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.AccessToken, token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,  // Nên dùng HTTPS
                SameSite = SameSiteMode.None, // Để trình duyệt cho phép chia sẻ cookie
                Expires = DateTime.UtcNow.AddDays(7)
            });

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.RefreshToken, token.RefreshToken, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,  // Nên dùng HTTPS
                HttpOnly = true,
                SameSite = SameSiteMode.None, // Để trình duyệt cho phép chia sẻ cookie
            });
        }
    }
}
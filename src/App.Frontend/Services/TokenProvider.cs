
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;

namespace App.Frontend.Services
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
            var cookieOptions = new CookieOptions { Expires = DateTime.UtcNow.AddDays(1) };
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.AccessToken, token.AccessToken, cookieOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.RefreshToken, token.RefreshToken, cookieOptions);
        }
    }
}
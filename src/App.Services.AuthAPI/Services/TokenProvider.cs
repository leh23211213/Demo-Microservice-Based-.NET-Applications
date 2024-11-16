using App.Services.AuthAPI.Models;
namespace App.Services.AuthAPI.Services
{
    public interface ITokenProvider
    {
        void SetToken(Token token);
        Token? GetToken();
        void ClearToken();
    }
    /// <summary>
    ///  working with cookies
    /// </summary>
    public class TokenProvider : ITokenProvider
    {
        private readonly string domain;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(
                            IConfiguration configuration,
                            IHttpContextAccessor httpContextAccessor
                            )
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            domain = _configuration.GetValue<string>("SetToken:domain");
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

            catch
            {
                return null;
            }
        }
        public void SetToken(Token token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.AccessToken, token.AccessToken ?? "", new CookieOptions
            {
                Domain = domain,
                Path = "/",
                HttpOnly = true,
                Secure = true,  // Nên dùng HTTPS
                SameSite = SameSiteMode.None, // Để trình duyệt cho phép chia sẻ cookie
                Expires = DateTime.UtcNow.AddDays(7)
            });
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.RefreshToken, token.RefreshToken ?? "", new CookieOptions
            {
                Domain = domain,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,  // Nên dùng HTTPS
                HttpOnly = true,
                SameSite = SameSiteMode.None, // Để trình duyệt cho phép chia sẻ cookie
            });
        }
    }
}
using App.Services.AuthAPI.Models;
<<<<<<< HEAD
using App.Services.AuthAPI.Services.IServices;
namespace App.Services.AuthAPI.Services
{
=======
using App.Services.AuthAPI.Utility;
namespace App.Services.AuthAPI.Services
{
    public interface ITokenProvider
    {
        void SetToken(Token token);
        Token? GetToken();
        void ClearToken();
    }
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
    /// <summary>
    ///  working with cookies
    /// </summary>
    public class TokenProvider : ITokenProvider
    {
<<<<<<< HEAD
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
=======
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
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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
<<<<<<< HEAD
            catch (Exception ex)
=======
            catch
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
            {
                return null;
            }
        }
        public void SetToken(Token token)
        {
<<<<<<< HEAD
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.AccessToken, token.AccessToken, new CookieOptions
            {
=======
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.AccessToken, token.AccessToken ?? "", new CookieOptions
            {
                Domain = domain,
                Path = "/",
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
                HttpOnly = true,
                Secure = true,  // Nên dùng HTTPS
                SameSite = SameSiteMode.None, // Để trình duyệt cho phép chia sẻ cookie
                Expires = DateTime.UtcNow.AddDays(7)
            });

<<<<<<< HEAD
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.RefreshToken, token.RefreshToken, new CookieOptions
            {
=======
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.RefreshToken, token.RefreshToken ?? "", new CookieOptions
            {
                Domain = domain,
                Path = "/",
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,  // Nên dùng HTTPS
                HttpOnly = true,
                SameSite = SameSiteMode.None, // Để trình duyệt cho phép chia sẻ cookie
            });
        }
    }
}
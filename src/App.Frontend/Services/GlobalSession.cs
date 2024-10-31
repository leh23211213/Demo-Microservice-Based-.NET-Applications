using App.Frontend.Services.IServices;

namespace App.Frontend.Services
{
    public class GlobalSession : IGlobalSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GlobalSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSessionData(string key, string value)
        {
            _httpContextAccessor.HttpContext?.Session.SetString(key, value);
        }

        public string GetSessionData(string key)
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(key);
        }
    }
}
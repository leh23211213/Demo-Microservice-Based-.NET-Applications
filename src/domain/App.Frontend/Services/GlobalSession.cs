<<<<<<< HEAD
using App.Frontend.Services.IServices;

namespace App.Frontend.Services
{
=======
namespace App.Frontend.Services
{
    public interface IGlobalSession
    {
        void SetSessionData(string key, string value);
        string GetSessionData(string key);
    }

>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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
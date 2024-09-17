using App.Frontend.Areas.Account.Models;
using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;

namespace App.Frontend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        private string _url;
        public AuthService(IBaseService baseService, IConfiguration configuration)
        {
            _baseService = baseService;
            _url = configuration.GetValue<string>("ServiceUrls:authAPI");

        }

        public async Task<Response?> LoginAsync(LoginRequest loginRequest)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = loginRequest,
                Url = _url + $"/api/{StaticDetail.CurrentAPIVersion}/auth/login"
            });
        }

        public async Task<Response?> RegisterAsync(RegistrationRequest registerRequest)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = registerRequest,
                Url = _url + $"/api/{StaticDetail.CurrentAPIVersion}/auth/register"
            }, withBearer: false);
        }
        // TODO : logout 
        public async Task<Response?> LogoutAsync(Token token)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = token,
                Url = _url + $"/api/{StaticDetail.CurrentAPIVersion}/UsersAuth/revoke"
            }, withBearer: false);
        }
    }
}
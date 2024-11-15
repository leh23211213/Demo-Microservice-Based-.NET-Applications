using App.Domain.Admin.Areas.Account.Models;
using App.Domain.Admin.Models;
using App.Domain.Admin.Services.IServices;
using App.Domain.Admin.Utility;

namespace App.Domain.Admin.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response?> LoginAsync(LoginRequest loginRequest)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = loginRequest,
                Url = StaticDetail.AuthAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/auth/Login"
            });
        }

        public async Task<Response?> RegisterAsync(RegistrationRequest registerRequest)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = registerRequest,
                Url = StaticDetail.AuthAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/auth/Register"
            }, withBearer: false);
        }

        public async Task<Response?> LogoutAsync(Token token)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = token,
                Url = StaticDetail.AuthAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/auth/Revoke"
            });
        }
    }
}
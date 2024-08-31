using App.Frontend.Models;
using App.Frontend.Services.IServices;
using App.Frontend.Utility;

namespace App.Frontend.Services
{
    public class AuthAPIService : IAuthAPIService
    {
        private readonly IBaseService _baseService;
        public AuthAPIService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response?> AssignRoleAsync(RegistrationRequest registrationRequest)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = registrationRequest,
                Url = StaticDetail.AuthAPIBase + "api/auth/AssignRole"
            }, withBearer: false);
        }

        public async Task<Response?> LoginAsync(LoginRequest loginRequest)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = loginRequest,
                Url = StaticDetail.AuthAPIBase + "api/auth/login"
            }, withBearer: false);  
        }

        public async Task<Response?> RegisterAsync(RegistrationRequest registerRequest)
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.POST,
                Data = registerRequest,
                Url = StaticDetail.AuthAPIBase + "api/auth/register"
            }, withBearer: false);
        }
    }
}
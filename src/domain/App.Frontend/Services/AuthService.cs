<<<<<<< HEAD
using App.Domain.Admin.Areas.Account.Models;
using App.Domain.Admin.Models;
using App.Domain.Admin.Services.IServices;
using App.Domain.Admin.Utility;

namespace App.Domain.Admin.Services
{
=======
using App.Frontend.Areas.Account.Models;
using App.Frontend.Models;
using App.Frontend.Utility;

namespace App.Frontend.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// If user already have token in cookie redirect to application again
        /// </summary>
        /// <returns></returns>
        Task<Response?> LoginAsync();
        Task<Response?> LoginAsync(LoginRequest loginRequest);
        Task<Response?> RegisterAsync(RegistrationRequest registerRequest);
        Task<Response?> LogoutAsync(Token token);
    }
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<Response?> LoginAsync()
        {
            return await _baseService.SendAsync(new Request()
            {
                ApiType = StaticDetail.ApiType.GET,
                Url = StaticDetail.AuthAPIBase + $"/api/{StaticDetail.CurrentAPIVersion}/auth/ReLogin"
            });
        }
<<<<<<< HEAD
=======

>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
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
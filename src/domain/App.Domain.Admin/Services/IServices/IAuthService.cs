
using App.Domain.Admin.Areas.Account.Models;
using App.Domain.Admin.Models;

namespace App.Domain.Admin.Services.IServices
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
}

using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IAuthService
    {
        /// <summary>
        /// If user already have token in cookie redirect to application again
        /// </summary>
        /// <returns></returns>
        Task<Response?> LoginAsync();
        Task<Response?> LoginAsync(LoginRequest loginRequest);
        // Task<Response?> RegisterAsync(RegistrationRequest registerRequest);
        Task<Response?> LogoutAsync(Token token);
    }
}
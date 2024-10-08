
using App.Frontend.Areas.Account.Models;
using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IAuthService
    {
        Task<Response?> LoginAsync(LoginRequest loginRequest);
        Task<Response?> RegisterAsync(RegistrationRequest registerRequest);
        Task<Response?> LogoutAsync(Token obj);
    }
}
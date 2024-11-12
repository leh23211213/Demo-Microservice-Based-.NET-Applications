
using App.Domain.Admin.Areas.Account.Models;
using App.Domain.Admin.Models;

namespace App.Domain.Admin.Services.IServices
{
    public interface IAuthService
    {
        Task<Response?> LoginAsync(LoginRequest loginRequest);
        Task<Response?> RegisterAsync(RegistrationRequest registerRequest);
        Task<Response?> LogoutAsync(Token token);
    }
}
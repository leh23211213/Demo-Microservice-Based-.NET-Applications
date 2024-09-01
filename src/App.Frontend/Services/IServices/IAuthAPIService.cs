
using App.Frontend.Areas.Account.Models;
using App.Frontend.Models;

namespace App.Frontend.Services.IServices
{
    public interface IAuthAPIService
    {
        Task<Response?> LoginAsync(LoginRequest loginRequest);
        Task<Response?> RegisterAsync(RegistrationRequest registerRequest);
        Task<Response?> AssignRoleAsync(RegistrationRequest registrationRequest);
    }
}
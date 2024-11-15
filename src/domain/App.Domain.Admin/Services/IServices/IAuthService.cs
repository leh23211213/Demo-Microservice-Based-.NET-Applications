
using App.Domain.Admin.Areas.Account.Models;
using App.Domain.Admin.Models;

namespace App.Domain.Admin.Services.IServices
{
    public interface IAuthService
    {
<<<<<<< HEAD
=======
        /// <summary>
        /// If user already have token in cookie redirect to application again
        /// </summary>
        /// <returns></returns>
        Task<Response?> LoginAsync();
>>>>>>> 34f0162eaa816ab08a78191cb4d003ff1457bee0
        Task<Response?> LoginAsync(LoginRequest loginRequest);
        Task<Response?> RegisterAsync(RegistrationRequest registerRequest);
        Task<Response?> LogoutAsync(Token token);
    }
}
using App.Services.AuthAPI.Models;

namespace App.Services.AuthAPI.Services.IServices
{
    public interface IAuthAPIService
    {
        Task<string> Register(RegistrationRequest RegistrationRequest);
        Task<Token> Login(LoginRequest loginRequest);
    }
}
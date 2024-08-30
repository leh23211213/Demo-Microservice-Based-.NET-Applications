using App.Services.AuthAPI.Models.DTOs;

namespace App.Services.AuthAPI.Services.IServices
{
    public interface IAuthAPIService
    {
        Task<string> Register(RegistrationRequest RegistrationRequest);
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<bool> AssignRole(string email, string roleName);
    }
}
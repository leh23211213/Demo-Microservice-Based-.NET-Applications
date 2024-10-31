using App.Services.AuthAPI.Models;

namespace App.Services.AuthAPI.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(Token token);
        Token? GetToken();
        void ClearToken();
    }
}
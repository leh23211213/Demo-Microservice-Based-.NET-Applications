using App.Frontend.Models;
namespace App.Frontend.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(Token token);
        Token? GetToken();
        void ClearToken();
    }
}
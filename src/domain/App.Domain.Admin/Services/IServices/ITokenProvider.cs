using App.Domain.Admin.Models;
namespace App.Domain.Admin.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(Token token);
        Token? GetToken();
        void ClearToken();
    }
}
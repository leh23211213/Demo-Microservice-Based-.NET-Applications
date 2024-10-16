
namespace App.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task RegisterUserEmailAndLog(string email);
    }
}

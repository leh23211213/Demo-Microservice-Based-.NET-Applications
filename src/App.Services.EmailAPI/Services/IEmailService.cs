
namespace App.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task RegisterUserEmailAndLog(string email);
        Task SendEmailAsync(string email, string subject, string message);
    }
}

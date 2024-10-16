using App.Services.EmailAPI.Data;
using App.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;

        public EmailService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = $"Congratulation {email}, You are registration successful";
            await LogAndEmail(message, email);
        }
        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSentTime = DateTime.Now,
                    Message = message,
                };

                await using var _db = new ApplicationDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
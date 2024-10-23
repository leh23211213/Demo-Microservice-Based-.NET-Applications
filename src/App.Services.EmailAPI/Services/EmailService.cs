using App.Services.EmailAPI.Data;
using App.Services.EmailAPI.Models;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace App.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;
        private readonly EmailLogger _mailSettings;
        // private readonly ILogger<EmailService> logger;
        public EmailService(
                            // IOptions<EmailLogger> mailSettings,
                            DbContextOptions<ApplicationDbContext> dbOptions
                            )
        {
            //  _mailSettings = mailSettings.Value;
            _dbOptions = dbOptions;
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = $"Congratulation {email}, You are registration successful";
            await RegisterAndEmail(message, email);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();
            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Email send fail will save in folder mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);
            }
            smtp.Disconnect(true);
        }

        private async Task<bool> RegisterAndEmail(string message, string email)
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
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VateraLite.Infrastructure.Settings;

namespace VateraLite.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
            {
                Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password),
                EnableSsl= true
            };

            await client.SendMailAsync(new MailMessage(_mailSettings.Mail, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            });
        }
    }
}

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ecommerce.Application.Services.Interfaces;

namespace Ecommerce.Infrastructure.Services
{
    public class SmtpEmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        public SmtpEmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var smtpSection = _configuration.GetSection("Smtp");
            var host = smtpSection["Host"];
            var port = int.Parse(smtpSection["Port"]);
            var enableSsl = bool.Parse(smtpSection["EnableSsl"]);
            var username = smtpSection["Username"];
            var password = smtpSection["Password"];

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl
            };
            var mail = new MailMessage(username, toEmail, subject, htmlMessage)
            {
                IsBodyHtml = true
            };
            await client.SendMailAsync(mail);
        }
    }
}

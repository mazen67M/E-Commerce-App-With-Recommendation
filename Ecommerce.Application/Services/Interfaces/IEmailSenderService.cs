using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
}

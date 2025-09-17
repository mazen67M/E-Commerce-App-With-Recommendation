using Ecommerce.Application.DTOs;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendOrderConfirmationEmailAsync(UserDto user, OrderDto order);
    }
}
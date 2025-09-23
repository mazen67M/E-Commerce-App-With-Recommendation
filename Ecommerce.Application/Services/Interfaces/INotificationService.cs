using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.DTOs.User;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendOrderConfirmationEmailAsync(UserDto user, OrderDto order);
    }
}
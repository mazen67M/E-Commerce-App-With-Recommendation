using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string body)
        {
            // In a real application, you would integrate with an email service like SendGrid, MailKit, etc.
            // For now, we will just log the email content to the console/log file.
            _logger.LogInformation("--- SENDING EMAIL ---");
            _logger.LogInformation($"To: {to}");
            _logger.LogInformation($"Subject: {subject}");
            _logger.LogInformation($"Body: {body}");
            _logger.LogInformation("---------------------");

            return Task.CompletedTask;
        }

        public async Task SendOrderConfirmationEmailAsync(UserDto user, OrderDto order)
        {
            string subject = $"Order Confirmation #{order.OrderID}";
            string body = $"<h1>Thank you for your order, {user.FirstName}!</h1>" +
                          $"<p>Your order with a total of {order.TotalAmount:C} has been received and is now being processed.</p>";

            await SendEmailAsync(user.Email, subject, body);
        }
    }
}

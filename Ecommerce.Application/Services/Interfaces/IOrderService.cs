using Ecommerce.Application.DTOs.Order;
using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId, string shippingAddress, string paymentMethod);
        Task<OrderDetailsDto> GetOrderDetailsAsync(int orderId);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task CancelOrderAsync(int orderId);
    }
}

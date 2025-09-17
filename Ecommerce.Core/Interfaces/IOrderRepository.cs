using Ecommerce.Core.Entities;
using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface IOrderRepository: IRepository<Order>
    {
        /// Gets all orders placed by a specific user.
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);

        /// Gets a single order including its OrderItems, Payment, and Shipping details.
        Task<Order?> GetOrderWithDetailsAsync(int orderId);

        /// Gets all orders with a specific status.
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);

        /// Gets orders placed within a specific date range.
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// Gets the most recent N orders.
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count);

        /// Gets orders associated with a specific payment method.
        Task<IEnumerable<Order>> GetOrdersByPaymentMethodAsync(PaymentMethod paymentMethod);

        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        
        Task<Dictionary<OrderStatus, int>> GetOrderStatusCountsAsync();
    }
}

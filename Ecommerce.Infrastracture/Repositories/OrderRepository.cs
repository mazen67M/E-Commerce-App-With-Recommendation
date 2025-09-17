using Ecommerce.Core.Entities;
using Ecommerce.Core.Enums;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) :base(context)
        {
            _context = context;
        }

        // Done
        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<Order>> GetOrdersByPaymentMethodAsync(PaymentMethod paymentMethod)
        {
            return await _context.Orders
                .Where(o => o.PaymentMethod == paymentMethod)
                .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserID == userId)
                .ToListAsync();
        }

        // Done
        public async Task<Dictionary<OrderStatus, int>> GetOrderStatusCountsAsync()
        {
            var statusCounts = await _context.Orders
               .GroupBy(o => o.Status)
               .Select(g => new { Status = g.Key, Count = g.Count() })
               .ToListAsync();

            return statusCounts.ToDictionary(x => x.Status, x => x.Count);
        }

        // Done
        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                 .Include(o => o.OrderItems)
                     .ThenInclude(oi => oi.Product)
                 .Include(o => o.Payment)
                 .Include(o => o.Shipping)
                 .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        // Done
        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count)
        {
            return await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        // Done
        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Payments.Where(p => p.PaymentStatus == PaymentStatus.Paid);

            if (startDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate <= endDate.Value);
            }

            // Sum the Amount property
            return await query.SumAsync(p => (decimal?)p.Amount) ?? 0m;
        }
    }
}

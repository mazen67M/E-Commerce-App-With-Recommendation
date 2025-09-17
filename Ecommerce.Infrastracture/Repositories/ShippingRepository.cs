using Ecommerce.Core.Entities;
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
    public class ShippingRepository : Repository<Shipping>, IShippingRepository
    {
        private readonly AppDbContext _context;
        public ShippingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        //Done
        public async Task<IEnumerable<Shipping>> GetRecentShippingsAsync(int count)
        {
            return await _context.Shippings
                            .Where(s => s.ShippedDate.HasValue) // Or OrderByDescending(s => s.ShippingID) if creation date is key
                            .OrderByDescending(s => s.ShippedDate)
                            .Take(count)
                            .ToListAsync();
        }

        //Done
        public async Task<Shipping?> GetShippingByOrderIdAsync(int orderId)
        {
            return await _context.Shippings
                .FirstOrDefaultAsync(s=>s.OrderID == orderId);
        }

        //Done
        public async Task<Shipping?> GetShippingByTrackingNumberAsync(string trackingNumber)
        {
            return await _context.Shippings
                .FirstOrDefaultAsync(s=>s.TrackingNumber != null && s.TrackingNumber == trackingNumber);
        }

        // Done
        public async Task<IEnumerable<Shipping>> GetShippingsByCourierAsync(string courierName)
        {
            return await _context.Shippings
                  .Where(s => s.CourierName != null && s.CourierName.Contains(courierName))
                  .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<Shipping>> GetShippingsByDateRangeAsync(DateTime startDate, DateTime endDate, string dateType = "Shipped")
        {
            switch (dateType.ToLower())
            {
                case "shipped":
                    return await _context.Shippings
                        .Where(s=>s.ShippedDate.HasValue && s.ShippedDate.Value >= startDate && s.ShippedDate<= endDate)
                        .ToListAsync();

                case "delivered":
                    return await _context.Shippings
                        .Where(s => s.ShippedDate.HasValue && s.ShippedDate.Value >= startDate && s.ShippedDate <= endDate)
                        .ToListAsync();

                case "confirmed":
                    return await _context.Shippings
                        .Where(s => s.ShippedDate.HasValue && s.ShippedDate.Value >= startDate && s.ShippedDate <= endDate)
                        .ToListAsync();

                case "pending":
                    return await _context.Shippings
                        .Where(s => s.ShippedDate.HasValue && s.ShippedDate.Value >= startDate && s.ShippedDate <= endDate)
                        .ToListAsync();

                case "cancelled":
                    return await _context.Shippings
                        .Where(s => s.ShippedDate.HasValue && s.ShippedDate.Value >= startDate && s.ShippedDate <= endDate)
                        .ToListAsync();

                default:
                    throw new ArgumentException("Invalid dateType. Use 'Shipped' or 'Delivered'.", nameof(dateType));

            }
        }

        // Done
        public async Task<IEnumerable<Shipping>> GetShippingsByStatusAsync(bool? isShipped = null, bool? isDelivered = null)
        {
            var query = _context.Shippings.AsQueryable();

            if (isShipped.HasValue)
            {
                if (isShipped.Value)
                    query = query.Where(s => s.ShippedDate.HasValue);
                else
                    query = query.Where(s => !s.ShippedDate.HasValue);
            }

            if (isDelivered.HasValue)
            {
                if (isDelivered.Value)
                    query = query.Where(s => s.DeliveryDate.HasValue);
                else
                    query = query.Where(s => !s.DeliveryDate.HasValue);
            }

            return await query.ToListAsync();
        }
    }
}

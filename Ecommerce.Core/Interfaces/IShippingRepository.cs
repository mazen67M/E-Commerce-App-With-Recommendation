using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface IShippingRepository : IRepository<Shipping>
    {
        /// Gets a shipping record by its associated Order ID.
        Task<Shipping?> GetShippingByOrderIdAsync(int orderId);

        /// Gets all shipping records for a specific courier/provider.
        Task<IEnumerable<Shipping>> GetShippingsByCourierAsync(string courierName);

        /// Gets all shipping records with a specific status (e.g., based on ShippedDate, DeliveryDate).
        Task<IEnumerable<Shipping>> GetShippingsByStatusAsync(bool? isShipped = null, bool? isDelivered = null);

        /// Gets shipping records within a specific date range (e.g., shipped or delivered dates).
        Task<IEnumerable<Shipping>> GetShippingsByDateRangeAsync(DateTime startDate, DateTime endDate, string dateType = "Shipped"); // dateType could be an enum

        /// Gets the N most recent shipping records (e.g., most recently shipped).
        Task<IEnumerable<Shipping>> GetRecentShippingsAsync(int count);

        /// Finds a shipping record by its tracking number.
        Task<Shipping?> GetShippingByTrackingNumberAsync(string trackingNumber);
    }
}

using Ecommerce.Core.Entities;
using Ecommerce.Core.Enums;

namespace Ecommerce.Core.Interfaces
{
    public interface IInventoryLogRepository : IRepository<InventoryLog>
    {
        /// Gets all inventory logs for a specific product.
        Task<IEnumerable<InventoryLog>> GetLogsByProductIdAsync(int productId);

        /// Gets all inventory logs of a specific change type.
        Task<IEnumerable<InventoryLog>> GetLogsByChangeTypeAsync(InventoryChangeType changeType);

        /// Gets inventory logs within a specific date range.
        Task<IEnumerable<InventoryLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// Gets the N most recent inventory log entries.
        Task<IEnumerable<InventoryLog>> GetRecentLogsAsync(int count);

    }
}

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
    public class InventoryLogRepository : Repository<InventoryLog>, IInventoryLogRepository
    {
        private readonly AppDbContext _context;
        public InventoryLogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Done
        public async Task<IEnumerable<InventoryLog>> GetLogsByChangeTypeAsync(InventoryChangeType changeType)
        {
            return await _context.InventoryLogs
                          .Where(log => log.ChangeType == changeType)
                          .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<InventoryLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.InventoryLogs
                     .Where(log => log.CreatedAt >= startDate && log.CreatedAt <= endDate)
                     .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<InventoryLog>> GetLogsByProductIdAsync(int productId)
        {

            return await _context.InventoryLogs
                .Where(log => log.ProductID == productId)
                .Include(log => log.Product) // Include product name if needed
                .ToListAsync();
        }

        // Done
        public  async Task<IEnumerable<InventoryLog>> GetRecentLogsAsync(int count)
        {
            return await _context.InventoryLogs
                            .OrderByDescending(log => log.CreatedAt)
                            .Take(count)
                            .Include(log => log.Product)
                            .ToListAsync();
        }
    }
}

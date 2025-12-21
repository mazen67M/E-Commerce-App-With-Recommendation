using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ActivityLogRepository : Repository<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ActivityLog>> GetRecentActivitiesAsync(int count)
        {
            return await _context.ActivityLogs
                .OrderByDescending(a => a.Timestamp)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivityLog>> GetUserActivitiesAsync(string userId, int count)
        {
            return await _context.ActivityLogs
                .Where(a => a.UserID == userId)
                .OrderByDescending(a => a.Timestamp)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivityLog>> GetActivitiesByActionAsync(string action, int count)
        {
            return await _context.ActivityLogs
                .Where(a => a.Action == action)
                .OrderByDescending(a => a.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}

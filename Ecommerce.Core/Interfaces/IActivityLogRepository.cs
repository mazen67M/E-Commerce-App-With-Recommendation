using Ecommerce.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface IActivityLogRepository : IRepository<ActivityLog>
    {
        Task<IEnumerable<ActivityLog>> GetRecentActivitiesAsync(int count);
        Task<IEnumerable<ActivityLog>> GetUserActivitiesAsync(string userId, int count);
        Task<IEnumerable<ActivityLog>> GetActivitiesByActionAsync(string action, int count);
    }
}

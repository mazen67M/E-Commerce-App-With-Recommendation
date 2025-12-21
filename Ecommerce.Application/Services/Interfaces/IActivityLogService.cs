using Ecommerce.Application.DTOs.ActivityLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task LogActivityAsync(string? userId, string action, string entityType, int? entityId = null, string? details = null);
        Task<IEnumerable<ActivityLogDto>> GetRecentActivitiesAsync(int count = 50);
        Task<IEnumerable<ActivityLogDto>> GetUserActivitiesAsync(string userId, int count = 50);
        Task<IEnumerable<ActivityLogDto>> GetActivitiesByActionAsync(string action, int count = 50);
    }
}

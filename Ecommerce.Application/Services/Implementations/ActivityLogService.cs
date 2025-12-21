using Ecommerce.Application.DTOs.ActivityLog;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivityLogService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActivityAsync(string? userId, string action, string entityType, int? entityId = null, string? details = null)
        {
            try
            {
                var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

                var log = new ActivityLog
                {
                    UserID = userId,
                    UserName = userName ?? "Anonymous",
                    Action = action,
                    EntityType = entityType,
                    EntityID = entityId,
                    Details = details,
                    IPAddress = ipAddress,
                    Timestamp = DateTime.Now
                };

                await _unitOfWork.ActivityLogs.AddAsync(log);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Log error but don't throw - activity logging should not break the application
            }
        }

        public async Task<IEnumerable<ActivityLogDto>> GetRecentActivitiesAsync(int count = 50)
        {
            var logs = await _unitOfWork.ActivityLogs.GetRecentActivitiesAsync(count);

            return logs.Select(l => new ActivityLogDto
            {
                ActivityLogID = l.ActivityLogID,
                UserID = l.UserID,
                UserName = l.UserName,
                Action = l.Action,
                EntityType = l.EntityType,
                EntityID = l.EntityID,
                Details = l.Details,
                IPAddress = l.IPAddress,
                Timestamp = l.Timestamp
            }).ToList();
        }
        public async Task<IEnumerable<ActivityLogDto>> GetUserActivitiesAsync(string userId, int count = 50)
        {
            var logs = await _unitOfWork.ActivityLogs.GetUserActivitiesAsync(userId, count);

            return logs.Select(l => new ActivityLogDto
            {
                ActivityLogID = l.ActivityLogID,
                UserID = l.UserID,
                UserName = l.UserName,
                Action = l.Action,
                EntityType = l.EntityType,
                EntityID = l.EntityID,
                Details = l.Details,
                IPAddress = l.IPAddress,
                Timestamp = l.Timestamp
            }).ToList();
        }

        public async Task<IEnumerable<ActivityLogDto>> GetActivitiesByActionAsync(string action, int count = 50)
        {
            var logs = await _unitOfWork.ActivityLogs.GetActivitiesByActionAsync(action, count);

            return logs.Select(l => new ActivityLogDto
            {
                ActivityLogID = l.ActivityLogID,
                UserID = l.UserID,
                UserName = l.UserName,
                Action = l.Action,
                EntityType = l.EntityType,
                EntityID = l.EntityID,
                Details = l.Details,
                IPAddress = l.IPAddress,
                Timestamp = l.Timestamp
            }).ToList();
        }
    }
}

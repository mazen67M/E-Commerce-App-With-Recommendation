using System;

namespace Ecommerce.Application.DTOs.ActivityLog
{
    public class ActivityLogDto
    {
        public int ActivityLogID { get; set; }
        public string? UserID { get; set; }
        public string? UserName { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public int? EntityID { get; set; }
        public string? Details { get; set; }
        public string? IPAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

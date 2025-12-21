using System;

namespace Ecommerce.Core.Entities
{
    public class ActivityLog
    {
        public int ActivityLogID { get; set; }
        public string? UserID { get; set; }
        public string? UserName { get; set; }
        public string Action { get; set; } = string.Empty; // "UserRegistered", "ProductAdded", "UserLogin", etc.
        public string EntityType { get; set; } = string.Empty; // "User", "Product", "Order"
        public int? EntityID { get; set; }
        public string? Details { get; set; }
        public string? IPAddress { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        public virtual ApplicationUser? User { get; set; }
    }
}

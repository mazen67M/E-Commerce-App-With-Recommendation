using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.Enums;

namespace Ecommerce.Core.Entities
{
    public class InventoryLog
    {
        [Key]  // Explicitly define as primary key
        public int LogID { get; set; }

        [Required]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        public InventoryChangeType ChangeType { get; set; }

        public int QuantityChanged { get; set; }
        public int NewStockLevel { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using Ecommerce.Core.Enums;
using System;

namespace Ecommerce.Application.DTOs.Inventory
{
    public class InventoryLogDto
    {
        public int LogID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty; // ✅ Updated
        public InventoryChangeType ChangeType { get; set; }
        public int QuantityChanged { get; set; }
        public int NewStockLevel { get; set; }
        public string? Description { get; set; } // Nullable is fine here
        public DateTime CreatedAt { get; set; }
    }
}
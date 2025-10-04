namespace Ecommerce.Application.DTOs.Reporting
{
    public class InventoryReportDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public int CurrentStock { get; set; }
        public decimal Price { get; set; }
        public bool IsLowStock { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class InventoryLogReportDto
    {
        public int LogId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ChangeType { get; set; }
        public int QuantityChanged { get; set; }
        public int NewStockLevel { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class InventorySummaryDto
    {
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int RecentStockChanges { get; set; }
    }
}

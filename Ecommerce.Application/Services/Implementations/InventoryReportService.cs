using Ecommerce.Application.DTOs.Reporting;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services.Implementations
{
    public class InventoryReportService : IInventoryReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InventorySummaryDto> GetInventorySummaryAsync()
        {
            var products = await _unitOfWork.Products.ListAllAsync();
            var recentLogs = await _unitOfWork.InventoryLogs.GetRecentLogsAsync(100);

            var lowStockThreshold = 10;
            var lowStockProducts = products.Where(p => p.StockQuantity <= lowStockThreshold && p.StockQuantity > 0).Count();
            var outOfStockProducts = products.Where(p => p.StockQuantity == 0).Count();
            var totalValue = products.Sum(p => p.Price * p.StockQuantity);
            var recentChanges = recentLogs.Where(l => l.CreatedAt >= DateTime.UtcNow.AddDays(-7)).Count();

            return new InventorySummaryDto
            {
                TotalProducts = products.Count(),
                LowStockProducts = lowStockProducts,
                OutOfStockProducts = outOfStockProducts,
                TotalInventoryValue = totalValue,
                RecentStockChanges = recentChanges
            };
        }

        public async Task<IEnumerable<InventoryReportDto>> GetInventoryReportAsync(int? lowStockThreshold = null)
        {
            var products = await _unitOfWork.Products.ListAllAsync();
            var threshold = lowStockThreshold ?? 10;

            return products.Select(p => new InventoryReportDto
            {
                ProductId = p.ProductID,
                ProductName = p.Name,
                CategoryName = p.Category?.Name ?? "N/A",
                BrandName = p.Brand?.Name ?? "N/A",
                CurrentStock = p.StockQuantity,
                Price = p.Price,
                IsLowStock = p.StockQuantity <= threshold && p.StockQuantity > 0,
                IsAvailable = p.IsAvailable,
                LastUpdated = p.UpdatedAt
            }).OrderBy(p => p.CurrentStock);
        }

        public async Task<IEnumerable<InventoryLogReportDto>> GetInventoryLogsAsync(DateTime? startDate = null, DateTime? endDate = null, int? productId = null)
        {
            var logs = productId.HasValue 
                ? await _unitOfWork.InventoryLogs.GetLogsByProductIdAsync(productId.Value)
                : await _unitOfWork.InventoryLogs.ListAllAsync();

            if (startDate.HasValue || endDate.HasValue)
            {
                var start = startDate ?? DateTime.MinValue;
                var end = endDate ?? DateTime.MaxValue;
                logs = logs.Where(l => l.CreatedAt >= start && l.CreatedAt <= end);
            }

            return logs.Select(l => new InventoryLogReportDto
            {
                LogId = l.LogID,
                ProductId = l.ProductID,
                ProductName = l.Product?.Name ?? "N/A",
                ChangeType = l.ChangeType.ToString(),
                QuantityChanged = l.QuantityChanged,
                NewStockLevel = l.NewStockLevel,
                Description = l.Description ?? "",
                CreatedAt = l.CreatedAt
            }).OrderByDescending(l => l.CreatedAt);
        }

        public async Task<IEnumerable<InventoryReportDto>> GetLowStockProductsAsync(int threshold = 10)
        {
            var products = await _unitOfWork.Products.GetLowStockAsync(threshold);

            return products.Select(p => new InventoryReportDto
            {
                ProductId = p.ProductID,
                ProductName = p.Name,
                CategoryName = p.Category?.Name ?? "N/A",
                BrandName = p.Brand?.Name ?? "N/A",
                CurrentStock = p.StockQuantity,
                Price = p.Price,
                IsLowStock = true,
                IsAvailable = p.IsAvailable,
                LastUpdated = p.UpdatedAt
            }).OrderBy(p => p.CurrentStock);
        }

        public async Task<IEnumerable<InventoryReportDto>> GetOutOfStockProductsAsync()
        {
            var products = await _unitOfWork.Products.ListAllAsync();
            var outOfStockProducts = products.Where(p => p.StockQuantity == 0);

            return outOfStockProducts.Select(p => new InventoryReportDto
            {
                ProductId = p.ProductID,
                ProductName = p.Name,
                CategoryName = p.Category?.Name ?? "N/A",
                BrandName = p.Brand?.Name ?? "N/A",
                CurrentStock = p.StockQuantity,
                Price = p.Price,
                IsLowStock = false,
                IsAvailable = p.IsAvailable,
                LastUpdated = p.UpdatedAt
            });
        }
    }
}

using Ecommerce.Application.DTOs.Reporting;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IInventoryReportService
    {
        Task<InventorySummaryDto> GetInventorySummaryAsync();
        Task<IEnumerable<InventoryReportDto>> GetInventoryReportAsync(int? lowStockThreshold = null);
        Task<IEnumerable<InventoryLogReportDto>> GetInventoryLogsAsync(DateTime? startDate = null, DateTime? endDate = null, int? productId = null);
        Task<IEnumerable<InventoryReportDto>> GetLowStockProductsAsync(int threshold = 10);
        Task<IEnumerable<InventoryReportDto>> GetOutOfStockProductsAsync();
    }
}

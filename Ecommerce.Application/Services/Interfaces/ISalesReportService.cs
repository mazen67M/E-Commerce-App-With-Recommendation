using Ecommerce.Application.DTOs.Reporting;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface ISalesReportService
    {
        Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<DailySalesDto>> GetDailySalesAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<TopSellingProductDto>> GetTopSellingProductsAsync(int count = 10, DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}

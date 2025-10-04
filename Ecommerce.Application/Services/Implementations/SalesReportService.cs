using Ecommerce.Application.DTOs.Reporting;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Enums;

namespace Ecommerce.Application.Services.Implementations
{
    public class SalesReportService : ISalesReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalesReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var orders = await GetOrdersInDateRange(startDate, endDate);
            var totalRevenue = await _unitOfWork.Orders.GetTotalRevenueAsync(startDate, endDate);
            var orderStatusCounts = await _unitOfWork.Orders.GetOrderStatusCountsAsync();

            var paymentMethodCounts = orders
                .GroupBy(o => o.PaymentMethod)
                .ToDictionary(g => g.Key, g => g.Count());

            var topProducts = await GetTopSellingProductsAsync(5, startDate, endDate);

            return new SalesSummaryDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = orders.Count(),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0,
                OrderStatusCounts = orderStatusCounts,
                PaymentMethodCounts = paymentMethodCounts,
                TopSellingProducts = topProducts.ToList()
            };
        }

        public async Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var orders = await GetOrdersInDateRange(startDate, endDate);

            return orders.Select(o => new SalesReportDto
            {
                OrderId = o.OrderID,
                UserId = o.UserID,
                UserEmail = o.User?.Email ?? "N/A",
                OrderDate = o.OrderDate,
                Status = o.Status,
                PaymentMethod = o.PaymentMethod,
                TotalAmount = o.TotalAmount,
                ItemCount = o.OrderItems?.Count ?? 0
            }).OrderByDescending(o => o.OrderDate);
        }

        public async Task<IEnumerable<DailySalesDto>> GetDailySalesAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var orders = await GetOrdersInDateRange(startDate, endDate);

            return orders
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new DailySalesDto
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount),
                    OrderCount = g.Count()
                })
                .OrderBy(d => d.Date);
        }

        public async Task<IEnumerable<TopSellingProductDto>> GetTopSellingProductsAsync(int count = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            var orders = await GetOrdersInDateRange(startDate, endDate);

            var productSales = orders
                .SelectMany(o => o.OrderItems ?? new List<Ecommerce.Core.Entities.OrderItem>())
                .GroupBy(oi => new { oi.ProductID, oi.Product?.Name })
                .Select(g => new TopSellingProductDto
                {
                    ProductId = g.Key.ProductID,
                    ProductName = g.Key.Name ?? "N/A",
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.UnitPrice * oi.Quantity)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(count);

            return productSales;
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _unitOfWork.Orders.GetTotalRevenueAsync(startDate, endDate);
        }

        private async Task<IEnumerable<Ecommerce.Core.Entities.Order>> GetOrdersInDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue || endDate.HasValue)
            {
                var start = startDate ?? DateTime.MinValue;
                var end = endDate ?? DateTime.MaxValue;
                return await _unitOfWork.Orders.GetOrdersByDateRangeAsync(start, end);
            }

            return await _unitOfWork.Orders.ListAllAsync();
        }
    }
}

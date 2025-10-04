using Ecommerce.Core.Enums;

namespace Ecommerce.Application.DTOs.Reporting
{
    public class SalesReportDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }

    public class SalesSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public Dictionary<OrderStatus, int> OrderStatusCounts { get; set; } = new();
        public Dictionary<PaymentMethod, int> PaymentMethodCounts { get; set; } = new();
        public List<TopSellingProductDto> TopSellingProducts { get; set; } = new();
    }

    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DailySalesDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}

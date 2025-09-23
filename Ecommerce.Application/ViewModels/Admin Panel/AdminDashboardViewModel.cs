using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.Admin_Panel
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TodayRevenue { get; set; }
        public List<OrderDetailsDto> RecentOrders { get; set; } = new();
        public List<ProductDto> LowStockProducts { get; set; } = new();
    }
}

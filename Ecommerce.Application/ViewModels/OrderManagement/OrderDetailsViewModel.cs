using Ecommerce.Application.DTOs.Products;
using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.OrderManagement
{
    public class OrderDetailsViewModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentViewModel Payment { get; set; }
        public ShippingViewModel Shipping { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();
        public List<ProductDto> RecommendedProducts { get; set; } = new(); // Post-purchase recommendations
    }

}

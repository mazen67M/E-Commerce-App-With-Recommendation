using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Shipping
{
    public class ShippingProviderDto
    {
        public int ProviderId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal BaseCost { get; set; }
        public decimal CostPerKg { get; set; }
        public int? EstimatedDeliveryDays { get; set; }
    }
}

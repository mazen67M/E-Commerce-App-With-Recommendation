using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Shipping
{
    public class ShippingQuoteDto
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = string.Empty;
        public string? ServiceLevel { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public int? EstimatedDeliveryDays { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public string? QuoteReference { get; set; }
        public bool IsCheapest { get; set; }
        public bool IsFastest { get; set; }
        public List<string> IncludedServices { get; set; } = new();
    }
}

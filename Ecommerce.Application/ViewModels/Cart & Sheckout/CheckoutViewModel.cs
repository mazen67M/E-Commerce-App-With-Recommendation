using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels
{
    public class CheckoutViewModel
    {
        public CartViewModel Cart { get; set; } = new();
        public string ShippingAddress { get; set; } = string.Empty;
        public string? BillingAddress { get; set; } 
        public PaymentMethod PaymentMethod { get; set; }
        public bool UseShippingAsBilling { get; set; } = true;
    }
}

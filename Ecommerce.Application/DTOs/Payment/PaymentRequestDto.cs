using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public int OrderID { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        // public string CardToken { get; set; } // Example for Stripe/PayPal
    }
}

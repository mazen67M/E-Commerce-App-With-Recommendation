﻿using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Payment
{
    public class PaymentResultDto
    {

        public bool IsSuccess { get; set; }
        public int? PaymentId { get; set; }
        public PaymentStatus Status { get; set; } // e.g., Paid, Failed, Pending
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; } // e.g., "Payment successful", "Card declined"
        public string RedirectUrl { get; set; }
    }
}

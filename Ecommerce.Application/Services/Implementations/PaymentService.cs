using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        public Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto paymentRequest)
        {
            throw new NotImplementedException();
        }
    }
}

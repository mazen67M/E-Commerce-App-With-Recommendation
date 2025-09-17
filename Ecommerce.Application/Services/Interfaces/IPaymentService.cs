using Ecommerce.Application.DTOs;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto paymentRequest);
    }
}
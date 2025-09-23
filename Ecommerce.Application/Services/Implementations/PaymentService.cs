using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Payment;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Enums;
using Ecommerce.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    // This is a MOCK implementation for development and testing.
    // In a real application, this service would integrate with a payment gateway API.
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IUnitOfWork unitOfWork, IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentResultDto> ProcessPaymentAsync(PaymentRequestDto paymentRequest)
        {
            var order = await _orderRepository.GetByIdAsync(paymentRequest.OrderID);
            if (order == null || order.TotalAmount != paymentRequest.Amount)
            {
                return new PaymentResultDto { IsSuccess = false, Message = "Invalid order or amount mismatch." };
            }

            // Simulate a successful API call to a payment gateway
            var transactionId = $"MOCK_PMT_{Guid.NewGuid().ToString().ToUpper()}";
            bool isPaymentSuccessful = true; // Always succeed for the mock

            if (isPaymentSuccessful)
            {
                var payment = new Payment
                {
                    OrderID = paymentRequest.OrderID,
                    PaymentDate = DateTime.UtcNow,
                    Amount = paymentRequest.Amount,
                    PaymentStatus = PaymentStatus.Paid,
                    TransactionID = transactionId
                };
                await _paymentRepository.AddAsync(payment);

                order.Status = OrderStatus.pending;
                await _orderRepository.UpdateAsync(order);

                await _unitOfWork.SaveChangesAsync();

                return new PaymentResultDto
                {
                    IsSuccess = true,
                    PaymentId = payment.PaymentID,
                    Status = payment.PaymentStatus,
                    TransactionId = transactionId,
                    Message = "Payment was successfully simulated."
                };
            }

            // This part would be executed if the simulated payment failed
            return new PaymentResultDto { IsSuccess = false, Status = PaymentStatus.Failed, Message = "Payment was declined by the provider." };
        }
    }
}
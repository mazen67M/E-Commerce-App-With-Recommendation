using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class ShippingService : IShippingService
    {
        public Task<decimal> CalculateShippingCostAsync(string fromAddress, string toAddress, decimal weight)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShippingProviderDto>> GetAvailableProvidersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ShippingQuoteDto> GetShippingQuoteAsync(ShippingRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<TrackingInfoDto> GetTrackingInfoAsync(string trackingNumber)
        {
            throw new NotImplementedException();
        }
    }
}

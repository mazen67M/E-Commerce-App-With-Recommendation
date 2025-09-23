using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class ValidationService : IValidationService
    {
        public Task<ValidationResultDto> ValidateCartForCheckoutAsync(CartDto cart)
        {
            throw new NotImplementedException();
        }
    }
}

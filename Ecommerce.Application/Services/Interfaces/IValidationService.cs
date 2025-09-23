using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Cart;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IValidationService
    {
        Task<ValidationResultDto> ValidateCartForCheckoutAsync(CartDto cart);
    }
}
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.DTOs.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<decimal> CalculateDiscountAsync(decimal originalAmount, string promoCode = null);
        Task<PromoCodeDto> GetPromoCodeAsync(string code);
        Task<bool> ValidatePromoCodeAsync(string code);
        Task UsePromoCodeAsync(string code);
        Task<IEnumerable<DiscountRuleDto>> GetApplicableDiscountsAsync(CartDto cart);
    }
}

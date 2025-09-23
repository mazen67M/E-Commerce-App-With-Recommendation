using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IPromoCodeService
    {
        Task<PromoCodeDto> GetPromoCodeByCodeAsync(string code);
        Task<bool> ValidatePromoCodeAsync(string code);
        Task<decimal> ApplyPromoCodeAsync(string code, decimal orderTotal);
        Task UsePromoCodeAsync(string code);
    }
}

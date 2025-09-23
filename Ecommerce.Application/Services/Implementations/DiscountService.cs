using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.DTOs.Promotion;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IMapper _mapper;

        public DiscountService(IUnitOfWork unitOfWork, IPromoCodeRepository promoCodeRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _promoCodeRepository = promoCodeRepository;
            _mapper = mapper;
        }

        // Done
        public async Task<decimal> CalculateDiscountAsync(decimal originalAmount, string promoCode = null)
        {
            if (string.IsNullOrEmpty(promoCode))
                return 0;

            if (await ValidatePromoCodeAsync(promoCode))
            {
                var promo= await _promoCodeRepository.GetPromoCodeByCodeAsync(promoCode);
                if (promo!.DiscountType == Core.Enums.DiscountType.Percentage)
                {
                    return originalAmount * (promo.DiscountValue / 100);
                }
                else if(promo.DiscountType == Core.Enums.DiscountType.FixedAmount)
                {
                    return promo.DiscountValue;
                }
            }
            return 0;
        }

        // Done
        public Task<IEnumerable<DiscountRuleDto>> GetApplicableDiscountsAsync(CartDto cart)
        {
            // Logic for automatic discounts (e.g., "10% off on orders over $200") would go here.
            // For now, returning an empty list.
            return Task.FromResult<IEnumerable<DiscountRuleDto>>(new List<DiscountRuleDto>());
        }

        // Done
        public async Task<PromoCodeDto> GetPromoCodeAsync(string code)
        {
            var promoCode =  await _promoCodeRepository.GetPromoCodeByCodeAsync(code);
            return _mapper.Map<PromoCodeDto>(promoCode);
        }

        // Done
        public async Task UsePromoCodeAsync(string code)
        {
            var promoCode = await _promoCodeRepository.GetPromoCodeByCodeAsync(code);
            if (promoCode != null)
            {
                await _promoCodeRepository.IncrementUsageCountAsync(promoCode.PromoCodeID);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        // Done
        public async Task<bool> ValidatePromoCodeAsync(string code)
        {
            var promoCode = await _promoCodeRepository.GetPromoCodeByCodeAsync(code);
            if (promoCode == null)
                return false;

            return promoCode.IsActive &&
                   promoCode.ExpirationDate > DateTime.UtcNow &&
                   promoCode.UsedCount < promoCode.MaxUsage;
        }

        Task<IEnumerable<DiscountRuleDto>> IDiscountService.GetApplicableDiscountsAsync(CartDto cart)
        {
            throw new NotImplementedException();
        }

    }
}

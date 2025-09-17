using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface IPromoCodeRepository : IRepository<PromoCode>
    {
        /// Gets a promo code by its unique code string.
        Task<PromoCode?> GetPromoCodeByCodeAsync(string code);

        /// Gets all active promo codes.
        Task<IEnumerable<PromoCode>> GetActivePromoCodesAsync();

        /// Gets active promo codes that are currently valid (between start/end dates).
        Task<IEnumerable<PromoCode>> GetCurrentlyValidPromoCodesAsync();

        /// Checks if a promo code has reached its maximum usage limit.
        Task<bool> IsPromoCodeFullyUsedAsync(int promoCodeId);

        /// Increments the UsedCount for a promo code.
        Task IncrementUsageCountAsync(int promoCodeId);

    }
}

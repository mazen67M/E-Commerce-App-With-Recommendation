using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class PromoCodeRepository : Repository<PromoCode>, IPromoCodeRepository
    {
        private readonly AppDbContext _context;
        public PromoCodeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Done
        public async Task<IEnumerable<PromoCode>> GetActivePromoCodesAsync()
        {
            return await _context.PromoCodes
                           .Where(pc => pc.IsActive)
                           .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<PromoCode>> GetCurrentlyValidPromoCodesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.PromoCodes
                .Where(pc => pc.IsActive &&
                             pc.ExpirationDate > now) // Assuming ExpirationDate is UTC
                .ToListAsync();
        }

        // Done
        public async Task<PromoCode?> GetPromoCodeByCodeAsync(string code)
        {
            return await _context.PromoCodes
                            .FirstOrDefaultAsync(pc => pc.Code == code);
        }

        // Done
        public async Task IncrementUsageCountAsync(int promoCodeId)
        {
            await _context.PromoCodes
                            .Where(pc => pc.PromoCodeID == promoCodeId)
                            .ExecuteUpdateAsync(setters => setters.SetProperty(pc => pc.UsedCount, pc => pc.UsedCount + 1));
        }

        // Done
        public async Task<bool> IsPromoCodeFullyUsedAsync(int promoCodeId)
        {
            var promoCode = await _context.PromoCodes.FindAsync(promoCodeId);
            if (promoCode == null) return true; // Treat non-existent as "used"
            return promoCode.UsedCount >= promoCode.MaxUsage;
        }
    }
}

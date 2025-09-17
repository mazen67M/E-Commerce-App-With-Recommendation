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
    public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Done
        public async Task<Wishlist?> GetWishlistByUserIdAsync(string userId)
        {
            return await _context.Wishlists
                           .Include(w => w.Items)
                               .ThenInclude(wi => wi.Product)
                           .FirstOrDefaultAsync(w => w.UserID == userId);
        }

        // Done
        public async Task<bool> IsProductInWishlistAsync(string userId, int productId)
        {
            return await _context.Wishlists
                            .AnyAsync(w => w.UserID == userId &&
                                           w.Items.Any(wi => wi.ProductID == productId));
        }
    }
}

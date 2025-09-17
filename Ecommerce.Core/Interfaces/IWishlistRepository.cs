using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
        /// Gets a user's wishlist, including the WishlistItems and related Product details.
        Task<Wishlist?> GetWishlistByUserIdAsync(string userId);

        /// Checks if a specific product is already in the user's wishlist.
        Task<bool> IsProductInWishlistAsync(string userId, int productId);

    }
}

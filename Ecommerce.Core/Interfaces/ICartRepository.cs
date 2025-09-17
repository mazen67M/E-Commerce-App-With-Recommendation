using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        /// Gets a user's cart, including the CartItems and related Product details.
        Task<Cart?> GetCartByUserIdAsync(string userId);

        /// Gets a specific cart including its items.
        Task<Cart?> GetCartWithItemsAsync(int cartId);

    }
}

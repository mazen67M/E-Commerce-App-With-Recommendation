using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        /// Finds a user by their email address.
        Task<ApplicationUser?> GetByEmailAsync(string email);

        /// Finds a user based on their associated Cart ID.
        Task<ApplicationUser?> GetByCartIdAsync(int cartId);

        /// Gets a user including their Cart and Cart Items.
        Task<ApplicationUser?> GetUserWithCartAsync(string userId);

        /// Gets a user including their Wishlist and Wishlist Items.
        Task<ApplicationUser?> GetUserWithWishlistAsync(string userId);

        /// Gets a user including their Reviews.
        Task<ApplicationUser?> GetUserWithReviewsAsync(string userId);

        /// Gets users who registered within a specific date range.
        Task<IEnumerable<ApplicationUser>> GetUsersRegisteredBetweenAsync(DateTime startDate, DateTime endDate);

        /// Gets users based on a specific role (if not using built-in Identity roles directly).
        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName);

        /// Gets users who have placed at least one order.
        Task<IEnumerable<ApplicationUser>> GetUsersWithOrdersAsync();

        /// Gets users who haven't placed any orders.
        Task<IEnumerable<ApplicationUser>> GetUsersWithoutOrdersAsync();
    }
}

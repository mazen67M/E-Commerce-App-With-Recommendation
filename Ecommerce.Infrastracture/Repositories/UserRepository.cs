using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Infrastructure.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly AppDbContext _context; 

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Done
        public async Task<ApplicationUser?> GetByCartIdAsync(int cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if(cart != null)
            {
                return await _context.Users.FindAsync(cart.UserID);
            }
            return null;
        }

        // Done
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Done
        public Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            throw new NotImplementedException("Use UserManager or adjust query for Identity roles. Direct EF query for Identity roles is complex.");
        }

        // Done
        public async Task<IEnumerable<ApplicationUser>> GetUsersRegisteredBetweenAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Users
                .Where(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate)
                .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<ApplicationUser>> GetUsersWithOrdersAsync()
        {
            return await _context.Users.Include(u=> u.Orders)
                                       .Where(u => u.Orders.Any())
                                       .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<ApplicationUser>> GetUsersWithoutOrdersAsync()
        {
            return await _context.Users
                            .Where(u => !u.Orders.Any()) 
                            .ToListAsync();
        }

        // Done
        public async Task<ApplicationUser?> GetUserWithCartAsync(string userId)
        {
            return await _context.Users
                 .Include(u => u.Cart) // Eagerly load the Cart navigation property
                     .ThenInclude(c => c.Items) // Then, eagerly load the Items within the Cart
                         .ThenInclude(ci => ci.Product) // Optionally, load Product details for each CartItem
                 .FirstOrDefaultAsync(u => u.Id == userId);
        }

        // Done
        public async Task<ApplicationUser?> GetUserWithReviewsAsync(string userId)
        {
            return await _context.Users
                           .Include(u => u.Reviews) // Eagerly load the Reviews navigation property
                               .ThenInclude(r => r.Product) // Optionally, load Product details for each Review
                           .FirstOrDefaultAsync(u => u.Id == userId);
        }

        // Done
        public async Task<ApplicationUser?> GetUserWithWishlistAsync(string userId)
        {
            return await _context.Users
                            .Include(u => u.Wishlist) // Eagerly load the Wishlist navigation property
                                .ThenInclude(w => w.Items) // Then, eagerly load the Items within the Wishlist
                                    .ThenInclude(wi => wi.Product) // Optionally, load Product details for each WishlistItem
                            .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}

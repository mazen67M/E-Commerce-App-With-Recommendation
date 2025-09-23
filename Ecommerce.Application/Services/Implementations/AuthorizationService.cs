using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Ecommerce.Application.Services.Interfaces;


namespace Ecommerce.Application.Services.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;

        public AuthorizationService(UserManager<ApplicationUser> userManager, IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        public async Task<bool> IsAdminAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Assuming you have an "Admin" role in your system
            return await _userManager.IsInRoleAsync(user, "Admin");
        }

        // This is an ownership-based authorization check
        public async Task<bool> CanUserAccessOrderAsync(string userId, int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false; // Order doesn't exist

            // A user can access the order if they are the owner OR if they are an admin.
            bool isOwner = order.UserID == userId;
            bool isAdmin = await IsAdminAsync(userId);

            return isOwner || isAdmin;
        }
    }
}
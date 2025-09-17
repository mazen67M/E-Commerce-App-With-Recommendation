using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetOrCreateCartAsync(string id);
        Task AddItemToCartAsync(string userId, int productId, int quantity);
        Task RemoveItemFromCartAsync(string userId, int cartId);
        Task UpdateItemQuantityAsync(string userId, int quantity,int cartItemId);
        Task ClearCartAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
    }
}

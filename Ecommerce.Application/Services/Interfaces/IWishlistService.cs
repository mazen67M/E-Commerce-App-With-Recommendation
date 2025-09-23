using Ecommerce.Application.DTOs.Wishlist;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistDto> GetWishlistAsync(string userId);

        Task AddToWishlistAsync(string userId, int productId);

        Task RemoveFromWishlistAsync(string userId, int productId);

        Task<bool> IsProductInWishlistAsync(string userId, int productId);
    }
}

using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels;
using Ecommerce.Application.ViewModels.User___Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommerce.Web.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;
        private readonly ICartService _cartService;

        public WishlistController(IWishlistService wishlistService, ICartService cartService)
        {
            _wishlistService = wishlistService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlist = await _wishlistService.GetWishlistAsync(userId);
            var viewModel = new WishlistViewModel
            {
                Items = wishlist.Items
            };
            return View(viewModel);
        }

        // Move item from wishlist to cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            try
            {
                // Add to cart
                await _cartService.AddItemToCartAsync(userId, productId, 1);
                
                // Remove from wishlist
                await _wishlistService.RemoveFromWishlistAsync(userId, productId);

                return Json(new { success = true, message = "Item moved to cart!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Remove item from wishlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            try
            {
                await _wishlistService.RemoveFromWishlistAsync(userId, productId);
                return Json(new { success = true, message = "Item removed from wishlist" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Move all items to cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveAllToCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            try
            {
                var wishlist = await _wishlistService.GetWishlistAsync(userId);
                int movedCount = 0;

                foreach (var item in wishlist.Items.ToList())
                {
                    await _cartService.AddItemToCartAsync(userId, item.ProductID, 1);
                    await _wishlistService.RemoveFromWishlistAsync(userId, item.ProductID);
                    movedCount++;
                }

                return Json(new { success = true, message = $"{movedCount} items moved to cart!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

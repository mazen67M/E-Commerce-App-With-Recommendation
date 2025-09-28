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

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
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

        // Note: The "Add to Wishlist" action would typically be in the main ProductController or a dedicated ApiController
    }
}
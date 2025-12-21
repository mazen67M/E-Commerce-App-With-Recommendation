using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Ecoomerce.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ICartService cartService,
            IProductService productService,
            ILogger<CartController> logger)
        {
            _cartService = cartService;
            _productService = productService;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var cart = await _cartService.GetOrCreateCartAsync(userId);
                
                // Populate product details for each cart item
                foreach (var item in cart.Items)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductID);
                    if (product != null)
                    {
                        item.ProductName = product.Name;
                        item.ImageURL = product.ImageURL;
                        item.UnitPrice = product.Price;
                    }
                }

                var viewModel = new CartViewModel
                {
                    Items = cart.Items,
                    Tax = cart.Tax,
                    Shipping = cart.Shipping,
                    Discount = cart.Discount
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart");
                TempData["Error"] = "Failed to load your cart. Please try again.";
                return View(new CartViewModel());
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                await _cartService.UpdateItemQuantityAsync(userId, productId, quantity);
                return Json(new { success = true, message = "Quantity updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item quantity");
                return Json(new { success = false, message = "Failed to update quantity" });
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                await _cartService.RemoveItemFromCartAsync(userId, productId);
                return Json(new { success = true, message = "Item removed from cart" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item");
                return Json(new { success = false, message = "Failed to remove item" });
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                await _cartService.ClearCartAsync(userId);
                return Json(new { success = true, message = "Cart cleared successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return Json(new { success = false, message = "Failed to clear cart" });
            }
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(0);
                }

                var cart = await _cartService.GetOrCreateCartAsync(userId);
                return Json(cart.Items.Count);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }
    }
}

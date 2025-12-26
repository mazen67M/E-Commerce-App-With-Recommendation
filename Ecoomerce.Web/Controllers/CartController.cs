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
        private readonly IWishlistService _wishlistService;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ICartService cartService,
            IProductService productService,
            IWishlistService wishlistService,
            ILogger<CartController> logger)
        {
            _cartService = cartService;
            _productService = productService;
            _wishlistService = wishlistService;
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
                        item.IsAvailable = product.IsAvailable;
                        item.StockQuantity = product.StockQuantity;
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

                // Check stock limit
                var product = await _productService.GetProductByIdAsync(productId);
                if (product != null && quantity > product.StockQuantity)
                {
                    return Json(new { success = false, message = $"Only {product.StockQuantity} items available in stock" });
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

        // Save for Later (move to wishlist)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveForLater(int productId)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                // Add to wishlist
                await _wishlistService.AddToWishlistAsync(userId, productId);
                
                // Remove from cart
                await _cartService.RemoveItemFromCartAsync(userId, productId);

                return Json(new { success = true, message = "Item saved for later!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving item for later");
                return Json(new { success = false, message = "Failed to save item" });
            }
        }

        // Mini Cart Preview (AJAX dropdown)
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MiniCart()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, items = new object[0], total = 0, count = 0 });
                }

                var cart = await _cartService.GetOrCreateCartAsync(userId);
                
                // Get product details
                var items = new List<object>();
                decimal total = 0;
                
                foreach (var item in cart.Items.Take(5)) // Limit to 5 for mini cart
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductID);
                    if (product != null)
                    {
                        var itemTotal = product.Price * item.Quantity;
                        total += itemTotal;
                        items.Add(new
                        {
                            productId = item.ProductID,
                            name = product.Name,
                            imageUrl = product.ImageURL,
                            price = product.Price,
                            quantity = item.Quantity,
                            total = itemTotal
                        });
                    }
                }

                return Json(new
                {
                    success = true,
                    items = items,
                    total = total,
                    count = cart.Items.Count,
                    hasMore = cart.Items.Count > 5
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading mini cart");
                return Json(new { success = false, items = new object[0], total = 0, count = 0 });
            }
        }
    }
}


using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly ICartService _cartService;
        private readonly IWishlistService _wishlistService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            IReviewService reviewService,
            ICartService cartService,
            IWishlistService wishlistService,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _reviewService = reviewService;
            _cartService = cartService;
            _wishlistService = wishlistService;
            _logger = logger;
        }

        // Action for the main products page (handles searching and filtering)
        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice, string? sortBy, int page = 1)
        {
            try 
            {
                _logger.LogInformation("Fetching products with search term: {SearchTerm}, category ID: {CategoryId}, page: {Page}", searchTerm, categoryId, page);
                
                var products = await _productService.SearchProductAsync(searchTerm, categoryId);
                
                // Apply price filtering if specified
                if (minPrice.HasValue)
                    products = products.Where(p => p.Price >= minPrice.Value);
                
                if (maxPrice.HasValue)
                    products = products.Where(p => p.Price <= maxPrice.Value);

                // Apply sorting
                products = sortBy?.ToLower() switch
                {
                    "price_asc" => products.OrderBy(p => p.Price),
                    "price_desc" => products.OrderByDescending(p => p.Price),
                    "name_asc" => products.OrderBy(p => p.Name),
                    "name_desc" => products.OrderByDescending(p => p.Name),
                    "rating" => products.OrderByDescending(p => p.AverageRating),
                    _ => products.OrderBy(p => p.Name)
                };

                // Pagination
                const int pageSize = 12;
                var totalProducts = products.Count();
                var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
                var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var viewModel = new ProductSearchViewModel
                {
                    Products = paginatedProducts,
                    SearchTerm = searchTerm,
                    CategoryId = categoryId,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    SortBy = sortBy,
                    CurrentPage = page,
                    TotalPages = totalPages
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products.");
                TempData["Error"] = "An error occurred while loading products. Please try again.";
                return View(new ProductSearchViewModel());
            }
        }

        // Action for product details page
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                _logger.LogInformation("Fetching product details for ID: {ProductId}", id);
                
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", id);
                    return NotFound();
                }

                // Get recommended products (same category - using a simple approach for now)
                var allProducts = await _productService.GetAllProductsAsync();
                var recommendedProducts = allProducts.Where(p => p.CategoryName == product.CategoryName && p.ProductID != id).Take(4).ToList();

                // Check if product is in wishlist (only for authenticated users)
                bool isInWishlist = false;
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        isInWishlist = await _wishlistService.IsProductInWishlistAsync(userId, id);
                    }
                }

                var viewModel = new ProductDetailsViewModel
                {
                    Product = product,
                    RecommendedProducts = recommendedProducts,
                    IsInWishlist = isInWishlist
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product details for ID: {ProductId}", id);
                TempData["Error"] = "An error occurred while loading product details. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Action for category-specific product listing
        public async Task<IActionResult> Category(int categoryId, int page = 1)
        {
            try
            {
                _logger.LogInformation("Fetching products for category ID: {CategoryId}, page: {Page}", categoryId, page);
                
                var products = await _productService.GetProductsByCategoryAsync(categoryId);
                
                // Pagination
                const int pageSize = 12;
                var totalProducts = products.Count();
                var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
                var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var viewModel = new ProductSearchViewModel
                {
                    Products = paginatedProducts,
                    CategoryId = categoryId,
                    CurrentPage = page,
                    TotalPages = totalPages
                };

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products for category ID: {CategoryId}", categoryId);
                TempData["Error"] = "An error occurred while loading category products. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // AJAX action for quick product search
        [HttpGet]
        public async Task<IActionResult> QuickSearch(string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
                {
                    return Json(new { success = false, message = "Search term too short" });
                }

                var products = await _productService.SearchProductAsync(term);
                var results = products.Take(5).Select(p => new
                {
                    id = p.ProductID,
                    name = p.Name,
                    price = p.Price.ToString("C"),
                    imageUrl = p.ImageURL,
                    url = Url.Action("Details", new { id = p.ProductID })
                });

                return Json(new { success = true, products = results });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during quick search for term: {SearchTerm}", term);
                return Json(new { success = false, message = "Search failed" });
            }
        }

        // Add product to cart
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                _logger.LogInformation("Adding product {ProductId} to cart for user {UserId}", productId, userId);

                // Verify product exists
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                if (!product.IsAvailable)
                {
                    return Json(new { success = false, message = "Product is out of stock" });
                }

                await _cartService.AddItemToCartAsync(userId, productId, quantity);

                return Json(new { success = true, message = "Product added to cart successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product {ProductId} to cart", productId);
                return Json(new { success = false, message = "Failed to add product to cart" });
            }
        }

        // Toggle wishlist (add/remove)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                _logger.LogInformation("Toggling wishlist for product {ProductId} and user {UserId}", productId, userId);

                // Check if product exists
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                // Check if already in wishlist
                bool isInWishlist = await _wishlistService.IsProductInWishlistAsync(userId, productId);

                if (isInWishlist)
                {
                    await _wishlistService.RemoveFromWishlistAsync(userId, productId);
                    return Json(new { success = true, isInWishlist = false, message = "Removed from wishlist" });
                }
                else
                {
                    await _wishlistService.AddToWishlistAsync(userId, productId);
                    return Json(new { success = true, isInWishlist = true, message = "Added to wishlist!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling wishlist for product {ProductId}", productId);
                return Json(new { success = false, message = "Failed to update wishlist" });
            }
        }

        // Check if product is in wishlist
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckWishlist(int productId)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, isInWishlist = false });
                }

                bool isInWishlist = await _wishlistService.IsProductInWishlistAsync(userId, productId);
                return Json(new { success = true, isInWishlist });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking wishlist for product {ProductId}", productId);
                return Json(new { success = false, isInWishlist = false });
            }
        }

        // Get featured products - View
        [HttpGet]
        public async Task<IActionResult> Featured(int count = 12)
        {
            try
            {
                _logger.LogInformation("Fetching {Count} featured products", count);
                var products = await _productService.GetFeaturedProductsAsync(count);
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching featured products");
                TempData["Error"] = "Failed to load featured products";
                return RedirectToAction(nameof(Index));
            }
        }

        // Get top selling products - View
        [HttpGet]
        public async Task<IActionResult> TopSelling(int count = 12)
        {
            try
            {
                _logger.LogInformation("Fetching {Count} top selling products", count);
                var products = await _productService.GetTopSellingProductsAsync(count);
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching top selling products");
                TempData["Error"] = "Failed to load top selling products";
                return RedirectToAction(nameof(Index));
            }
        }

        // Get featured products - AJAX/API
        [HttpGet]
        public async Task<IActionResult> GetFeaturedProducts(int count = 8)
        {
            try
            {
                var products = await _productService.GetFeaturedProductsAsync(count);
                return Json(new { success = true, products });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching featured products");
                return Json(new { success = false, message = "Failed to load featured products" });
            }
        }

        // Get top selling products - AJAX/API
        [HttpGet]
        public async Task<IActionResult> GetTopSellingProducts(int count = 8)
        {
            try
            {
                var products = await _productService.GetTopSellingProductsAsync(count);
                return Json(new { success = true, products });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching top selling products");
                return Json(new { success = false, message = "Failed to load top selling products" });
            }
        }

        // Compare products
        [HttpGet]
        public async Task<IActionResult> Compare(int[] productIds)
        {
            try
            {
                if (productIds == null || productIds.Length < 2)
                {
                    TempData["Error"] = "Please select at least 2 products to compare";
                    return RedirectToAction(nameof(Index));
                }

                if (productIds.Length > 4)
                {
                    TempData["Error"] = "You can compare up to 4 products at a time";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogInformation("Comparing products: {ProductIds}", string.Join(", ", productIds));

                var products = new List<Ecommerce.Application.DTOs.Products.ProductDto>();
                foreach (var id in productIds)
                {
                    var product = await _productService.GetProductByIdAsync(id);
                    if (product != null)
                    {
                        products.Add(product);
                    }
                }

                if (products.Count < 2)
                {
                    TempData["Error"] = "Not enough valid products to compare";
                    return RedirectToAction(nameof(Index));
                }

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error comparing products");
                TempData["Error"] = "An error occurred while comparing products";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

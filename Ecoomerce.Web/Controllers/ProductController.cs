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
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            IReviewService reviewService,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _reviewService = reviewService;
            _logger = logger;
        }

        // Action for the main products page (handles searching and filtering)
        public async Task<IActionResult> Index(string? searchTerm, int? categoryId)
        {
            try 
            {
                _logger.LogInformation("Fetching products with search term : {SearchTerm} and category ID: {CategoryId}", searchTerm, categoryId);
                var products = await _productService.SearchProductAsync(searchTerm, categoryId);

                var viewModel = new ProductSearchViewModel
                {
                    Products = products.ToList(),
                    SearchTerm = searchTerm,
                    CategoryId = categoryId
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products.");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

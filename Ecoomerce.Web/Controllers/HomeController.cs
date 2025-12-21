using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels;
using Ecoomerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecoomerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IRecommendationService _recommendationService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IProductService productService,
            IRecommendationService recommendationService,
            ILogger<HomeController> logger)
        {
            _productService = productService;
            _recommendationService = recommendationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching featured products for homepage.");

            try {
                var featuredProducts = await _productService.GetFeaturedProductsAsync(10);
                var viewModel = new HomeViewModel
                {
                    FeaturedProducts = featuredProducts.ToList()
                };
                return View(viewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching featured products.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            _logger.LogInformation("Fetching product details for product ID: {ProductId}", id);
            try
            {

                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID: {ProductId} not found.", id);
                    return NotFound();
                }
                var relatedProducts = await _recommendationService.GetRelatedProductsAsync(id, 5);
                var viewModel = new ProductDetailsViewModel
                {
                    Product = product,
                    RecommendedProducts = relatedProducts.ToList()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product details for product ID: {ProductId}", id);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Shipping()
        {
            return View();
        }

        public IActionResult Returns()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

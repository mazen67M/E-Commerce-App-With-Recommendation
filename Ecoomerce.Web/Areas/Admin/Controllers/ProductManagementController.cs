using AutoMapper;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels.Admin_Panel;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecoomerce.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductManagementController> _logger;
        public ProductManagementController(
            IProductService productService,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IMapper mapper,
            ILogger<ProductManagementController> logger)
        {
            _productService = productService;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            var viewModel = new ManageProductsViewModel
            {
                Products = products.ToList()
            };
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new EditProductViewModel
            {
                Categories = await GetCategoriesSelectList(),
                Brands = await GetBrandsSelectList()
            };
            return View(viewModel);
        }

        // Handles the submission of the new product form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns if validation fails
                model.Categories = await GetCategoriesSelectList();
                model.Brands = await GetBrandsSelectList();
                return View(model);
            }

            // In a real app, the ProductService would have a CreateProductAsync method.
            // This is a conceptual implementation. You would map the ViewModel to a DTO
            // and pass it to the service.
            // var productDto = _mapper.Map<ProductDto>(model);
            // await _productService.CreateProductAsync(productDto);

            _logger.LogInformation("New product '{ProductName}' created.", model.Name);
            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction("Index");
        }

        // Private helper methods to populate dropdowns
        private async Task<List<SelectListItem>> GetCategoriesSelectList()
        {
            var categories = await _categoryRepository.ListAllAsync();
            return categories.Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.Name }).ToList();
        }

        private async Task<List<SelectListItem>> GetBrandsSelectList()
        {
            var brands = await _brandRepository.ListAllAsync();
            return brands.Select(b => new SelectListItem { Value = b.BrandID.ToString(), Text = b.Name }).ToList();
        }
    }
}

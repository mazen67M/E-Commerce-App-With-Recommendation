using AutoMapper;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecoomerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandManagementController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandManagementController> _logger;

        public BrandManagementController(
            IBrandRepository brandRepository,
            IMapper mapper,
            ILogger<BrandManagementController> logger)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: Brand Management Index
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            try
            {
                var brands = await _brandRepository.ListAllAsync();
                var brandDtos = _mapper.Map<List<BrandDto>>(brands);

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    brandDtos = brandDtos.Where(b => b.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                    ViewBag.SearchTerm = searchTerm;
                }

                return View(brandDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading brands");
                TempData["ErrorMessage"] = "Failed to load brands.";
                return View(new List<BrandDto>());
            }
        }

        // GET: Create Brand
        [HttpGet]
        public IActionResult Create()
        {
            return View(new BrandDto());
        }

        // POST: Create Brand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Check if brand name already exists
                var existingBrands = await _brandRepository.ListAllAsync();
                if (existingBrands.Any(b => b.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("Name", "A brand with this name already exists.");
                    return View(model);
                }

                var brand = _mapper.Map<Ecommerce.Core.Entities.Brand>(model);
                await _brandRepository.AddAsync(brand);

                _logger.LogInformation("Brand '{BrandName}' created successfully", model.Name);
                TempData["SuccessMessage"] = $"Brand '{model.Name}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating brand '{BrandName}'", model.Name);
                TempData["ErrorMessage"] = "Failed to create brand. Please try again.";
                return View(model);
            }
        }

        // GET: Edit Brand
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var brand = await _brandRepository.GetByIdAsync(id);
                if (brand == null)
                {
                    TempData["ErrorMessage"] = "Brand not found.";
                    return RedirectToAction(nameof(Index));
                }

                var brandDto = _mapper.Map<BrandDto>(brand);
                return View(brandDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading brand {BrandId} for editing", id);
                TempData["ErrorMessage"] = "Failed to load brand for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Edit Brand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Check if brand name already exists (excluding current brand)
                var existingBrands = await _brandRepository.ListAllAsync();
                if (existingBrands.Any(b => b.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase) && b.BrandID != model.BrandID))
                {
                    ModelState.AddModelError("Name", "A brand with this name already exists.");
                    return View(model);
                }

                var brand = _mapper.Map<Ecommerce.Core.Entities.Brand>(model);
                await _brandRepository.UpdateAsync(brand);

                _logger.LogInformation("Brand '{BrandName}' (ID: {BrandId}) updated successfully", model.Name, model.BrandID);
                TempData["SuccessMessage"] = $"Brand '{model.Name}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating brand '{BrandName}' (ID: {BrandId})", model.Name, model.BrandID);
                TempData["ErrorMessage"] = "Failed to update brand. Please try again.";
                return View(model);
            }
        }

        // GET: Brand Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var brand = await _brandRepository.GetByIdAsync(id);
                if (brand == null)
                {
                    TempData["ErrorMessage"] = "Brand not found.";
                    return RedirectToAction(nameof(Index));
                }

                var brandDto = _mapper.Map<BrandDto>(brand);
                return View(brandDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading brand details for ID: {BrandId}", id);
                TempData["ErrorMessage"] = "Failed to load brand details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Delete Brand
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var brand = await _brandRepository.GetByIdAsync(id);
                if (brand == null)
                {
                    return Json(new { success = false, message = "Brand not found." });
                }

                // TODO: Check if brand has associated products
                // var hasProducts = await _productService.HasProductsWithBrandAsync(id);
                // if (hasProducts)
                // {
                //     return Json(new { success = false, message = "Cannot delete brand with associated products." });
                // }

                await _brandRepository.DeleteAsync(brand);

                _logger.LogInformation("Brand '{BrandName}' (ID: {BrandId}) deleted successfully", brand.Name, id);
                return Json(new { success = true, message = $"Brand '{brand.Name}' deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting brand with ID: {BrandId}", id);
                return Json(new { success = false, message = "Failed to delete brand. Please try again." });
            }
        }

        // AJAX: Get brands for dropdown
        [HttpGet]
        public async Task<IActionResult> GetBrandsJson()
        {
            try
            {
                var brands = await _brandRepository.ListAllAsync();
                var brandDtos = _mapper.Map<List<BrandDto>>(brands);
                return Json(new { success = true, brands = brandDtos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching brands for JSON");
                return Json(new { success = false, message = "Failed to load brands" });
            }
        }

        // AJAX: Check if brand name exists
        [HttpGet]
        public async Task<IActionResult> CheckBrandName(string name, int? excludeId = null)
        {
            try
            {
                var brands = await _brandRepository.ListAllAsync();
                var exists = brands.Any(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                                           (excludeId == null || b.BrandID != excludeId));
                
                return Json(new { exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking brand name availability");
                return Json(new { exists = false });
            }
        }
    }
}

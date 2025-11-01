using AutoMapper;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecoomerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryManagementController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryManagementController> _logger;

        public CategoryManagementController(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryManagementController> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: Category Management Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryRepository.ListAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
                
                // Build hierarchy
                var rootCategories = categoryDtos.Where(c => c.ParentCategoryID == null).ToList();
                foreach (var category in rootCategories)
                {
                    category.SubCategories = categoryDtos.Where(c => c.ParentCategoryID == category.CategoryID).ToList();
                }

                return View(rootCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories");
                TempData["ErrorMessage"] = "Failed to load categories.";
                return View(new List<CategoryDto>());
            }
        }

        // GET: Create Category
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryRepository.ListAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
                
                ViewBag.ParentCategories = new SelectList(categoryDtos, "CategoryID", "Name");
                return View(new CategoryDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading parent categories for create");
                TempData["ErrorMessage"] = "Failed to load parent categories.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Create Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDto model)
        {
            // Debug: Log all model values and form data
            _logger.LogInformation("Create Category - Received model: Name='{Name}', ParentCategoryID={ParentCategoryID}, CategoryID={CategoryID}", 
                model?.Name ?? "NULL", model?.ParentCategoryID, model?.CategoryID);
            
            // Debug: Log raw form data
            foreach (var formField in Request.Form)
            {
                _logger.LogInformation("Form Field: {Key} = '{Value}'", formField.Key, formField.Value);
            }

            if (!ModelState.IsValid)
            {
                // Debug: Log all validation errors
                _logger.LogWarning("Model validation failed for category creation:");
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        _logger.LogWarning("Validation Error - Field: {Field}, Error: {Error}", error.Key, subError.ErrorMessage);
                    }
                }

                var categories = await _categoryRepository.ListAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
                ViewBag.ParentCategories = new SelectList(categoryDtos, "CategoryID", "Name");
                
                // Add validation errors to TempData for debugging
                TempData["ValidationErrors"] = string.Join("; ", ModelState
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => $"{x.ErrorMessage}"));
                
                return View(model);
            }

            try
            {
                _logger.LogInformation("Attempting to create category: {CategoryName}", model.Name);
                
                var category = _mapper.Map<Ecommerce.Core.Entities.Category>(model);
                
                _logger.LogInformation("Category entity mapped, calling repository AddAsync...");
                
                var savedCategory = await _categoryRepository.AddAsync(category);

                _logger.LogInformation("Category saved successfully with ID: {CategoryId}", savedCategory.CategoryID);
                
                // Verify the category was saved
                var verifyCategory = await _categoryRepository.GetByIdAsync(savedCategory.CategoryID);
                if (verifyCategory != null)
                {
                    _logger.LogInformation("Category verification successful: {CategoryName} (ID: {CategoryId})", verifyCategory.Name, verifyCategory.CategoryID);
                    TempData["SuccessMessage"] = $"Category '{model.Name}' created successfully with ID {savedCategory.CategoryID}!";
                }
                else
                {
                    _logger.LogWarning("Category was created but verification failed for ID: {CategoryId}", savedCategory.CategoryID);
                    TempData["SuccessMessage"] = $"Category '{model.Name}' created but verification failed!";
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category '{CategoryName}'", model.Name);
                TempData["ErrorMessage"] = "Failed to create category. Please try again.";
                
                var categories = await _categoryRepository.ListAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
                ViewBag.ParentCategories = new SelectList(categoryDtos, "CategoryID", "Name");
                return View(model);
            }
        }

        // GET: Edit Category
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Category not found.";
                    return RedirectToAction(nameof(Index));
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);
                
                var allCategories = await _categoryRepository.ListAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(allCategories);
                
                // Exclude current category and its descendants from parent options
                var availableParents = categoryDtos.Where(c => c.CategoryID != id && !IsDescendant(c, id, categoryDtos)).ToList();
                ViewBag.ParentCategories = new SelectList(availableParents, "CategoryID", "Name", categoryDto.ParentCategoryID);
                
                return View(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading category {CategoryId} for editing", id);
                TempData["ErrorMessage"] = "Failed to load category for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Edit Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDto model)
        {
            if (!ModelState.IsValid)
            {
                var allCategories = await _categoryRepository.ListAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(allCategories);
                var availableParents = categoryDtos.Where(c => c.CategoryID != model.CategoryID).ToList();
                ViewBag.ParentCategories = new SelectList(availableParents, "CategoryID", "Name", model.ParentCategoryID);
                return View(model);
            }

            try
            {
                var category = _mapper.Map<Ecommerce.Core.Entities.Category>(model);
                await _categoryRepository.UpdateAsync(category);

                _logger.LogInformation("Category '{CategoryName}' (ID: {CategoryId}) updated successfully", model.Name, model.CategoryID);
                TempData["SuccessMessage"] = $"Category '{model.Name}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category '{CategoryName}' (ID: {CategoryId})", model.Name, model.CategoryID);
                TempData["ErrorMessage"] = "Failed to update category. Please try again.";
                
                var allCategories = await _categoryRepository.ListAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(allCategories);
                var availableParents = categoryDtos.Where(c => c.CategoryID != model.CategoryID).ToList();
                ViewBag.ParentCategories = new SelectList(availableParents, "CategoryID", "Name", model.ParentCategoryID);
                return View(model);
            }
        }

        // GET: Category Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Category not found.";
                    return RedirectToAction(nameof(Index));
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);
                
                // Get all categories to build hierarchy
                var allCategories = await _categoryRepository.ListAllAsync();
                var allCategoryDtos = _mapper.Map<List<CategoryDto>>(allCategories);
                
                // Get subcategories
                categoryDto.SubCategories = allCategoryDtos.Where(c => c.ParentCategoryID == id).ToList();
                
                // Get parent category name
                if (categoryDto.ParentCategoryID.HasValue)
                {
                    var parentCategory = allCategoryDtos.FirstOrDefault(c => c.CategoryID == categoryDto.ParentCategoryID.Value);
                    categoryDto.ParentCategoryName = parentCategory?.Name;
                }

                return View(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading category details for ID: {CategoryId}", id);
                TempData["ErrorMessage"] = "Failed to load category details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Delete Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Category not found." });
                }

                // Check if category has subcategories
                var allCategories = await _categoryRepository.ListAllAsync();
                var hasSubcategories = allCategories.Any(c => c.ParentCategoryID == id);
                
                if (hasSubcategories)
                {
                    return Json(new { success = false, message = "Cannot delete category with subcategories. Please delete subcategories first." });
                }

                await _categoryRepository.DeleteAsync(category);

                _logger.LogInformation("Category '{CategoryName}' (ID: {CategoryId}) deleted successfully", category.Name, id);
                return Json(new { success = true, message = $"Category '{category.Name}' deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID: {CategoryId}", id);
                return Json(new { success = false, message = "Failed to delete category. Please try again." });
            }
        }

        // Helper method to check if a category is a descendant of another
        private bool IsDescendant(CategoryDto category, int ancestorId, List<CategoryDto> allCategories)
        {
            if (category.ParentCategoryID == ancestorId)
                return true;

            if (category.ParentCategoryID.HasValue)
            {
                var parent = allCategories.FirstOrDefault(c => c.CategoryID == category.ParentCategoryID.Value);
                if (parent != null)
                    return IsDescendant(parent, ancestorId, allCategories);
            }

            return false;
        }

        // AJAX: Get categories for dropdown
        [HttpGet]
        public async Task<IActionResult> GetCategoriesJson()
        {
            try
            {
                var categories = await _categoryRepository.ListAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
                return Json(new { success = true, categories = categoryDtos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories for JSON");
                return Json(new { success = false, message = "Failed to load categories" });
            }
        }

        // DEBUG: Test database connection and save
        [HttpGet]
        public async Task<IActionResult> TestDatabaseSave()
        {
            try
            {
                // Test creating a simple category
                var testCategory = new Ecommerce.Core.Entities.Category
                {
                    Name = $"Test Category {DateTime.Now:HHmmss}",
                    ParentCategoryID = null
                };

                var savedCategory = await _categoryRepository.AddAsync(testCategory);
                
                _logger.LogInformation("Test category created with ID: {CategoryId}", savedCategory.CategoryID);
                
                // Verify it was saved by retrieving it
                var retrievedCategory = await _categoryRepository.GetByIdAsync(savedCategory.CategoryID);
                
                if (retrievedCategory != null)
                {
                    return Json(new { 
                        success = true, 
                        message = $"Database save test successful! Created category with ID: {savedCategory.CategoryID}",
                        categoryId = savedCategory.CategoryID,
                        categoryName = retrievedCategory.Name
                    });
                }
                else
                {
                    return Json(new { 
                        success = false, 
                        message = "Category was created but could not be retrieved - possible database issue" 
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database save test failed");
                return Json(new { 
                    success = false, 
                    message = $"Database save test failed: {ex.Message}",
                    details = ex.InnerException?.Message
                });
            }
        }
    }
}

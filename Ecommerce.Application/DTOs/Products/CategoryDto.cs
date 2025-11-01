using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Application.DTOs.Products
{
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        public int? ParentCategoryID { get; set; }
        
        [BindNever]
        public string? ParentCategoryName { get; set; }
        
        [BindNever]
        public List<CategoryDto> SubCategories { get; set; } = new();
    }
}

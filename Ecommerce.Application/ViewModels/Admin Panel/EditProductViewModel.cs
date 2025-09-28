using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectListItem
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.ViewModels.Admin_Panel
{
    public class EditProductViewModel
    {
        public int ProductID { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty; 

        public string? Description { get; set; } 

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
        public string? ImageURL { get; set; } 

        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Brand")]
        public int? BrandID { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Brands { get; set; } = new();
        public List<SelectListItem> Tags { get; set; } = new();

        public List<int> SelectedTagIds { get; set; } = new();
    }
}
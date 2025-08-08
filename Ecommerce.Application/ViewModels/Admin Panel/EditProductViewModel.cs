using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.Admin_Panel
{
    public class EditProductViewModel
    {
        public int ProductID { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageURL { get; set; }
        public int CategoryID { get; set; }
        public int? BrandID { get; set; }
        public List<Category> Categories { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
        public List<int> SelectedTagIds { get; set; } = new(); // For multi-select
    }
}

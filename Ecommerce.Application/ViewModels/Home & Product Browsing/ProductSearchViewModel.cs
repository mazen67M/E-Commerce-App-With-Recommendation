using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels
{
    public class ProductSearchViewModel
    {
        public List<ProductDto> Products { get; set; } = new();
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortBy { get; set; } // e.g., "price_asc", "name_desc"
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
    }
}

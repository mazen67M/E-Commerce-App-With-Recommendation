using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? BrandID { get; set; }
        public string BrandName { get; set; }
        public List<string> Tags { get; set; } = new();
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}

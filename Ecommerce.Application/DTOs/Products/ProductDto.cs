using System.Collections.Generic;

namespace Ecommerce.Application.DTOs.Products
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageURL { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int? BrandID { get; set; }
        public string? BrandName { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
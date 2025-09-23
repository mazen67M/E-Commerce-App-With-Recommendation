using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductDto Product { get; set; }
        public List<ProductDto> RecommendedProducts { get; set; } = new();
        public bool IsInWishlist { get; set; }
        public ReviewDto NewReview { get; set; } = new(); // For the review form
    }
}

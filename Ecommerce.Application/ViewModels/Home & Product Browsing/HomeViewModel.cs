using Ecommerce.Application.DTOs;
using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels
{
    public class HomeViewModel
    {
        public List<ProductDto> FeaturedProducts { get; set; } = new();
        public List<ProductDto> TopSellingProducts { get; set; } = new();
        public List<ProductDto> NewArrivals { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
    }
}

using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.Admin_Panel
{
    public class ManageProductsViewModel
    {
        public List<ProductDto> Products { get; set; } = new();
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
    }
}

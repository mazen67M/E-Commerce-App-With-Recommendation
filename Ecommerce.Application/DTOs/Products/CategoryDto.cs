using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Products
{
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryID { get; set; }
        public string ParentCategoryName { get; set; }
        public List<CategoryDto> SubCategories { get; set; } = new();

    }
}

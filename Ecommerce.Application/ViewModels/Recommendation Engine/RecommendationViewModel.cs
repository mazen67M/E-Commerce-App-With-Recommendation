using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.Recommendation_Engine
{
    public class RecommendationViewModel
    {
        public string Title { get; set; }
        public List<ProductDto> Products { get; set; } = new();
    }
}

using Ecommerce.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<ProductDto>> GetPersonalizedRecommendationsAsync(string userId);
        Task<IEnumerable<ProductDto>> GetRelatedProductsAsync(int productId, int count = 5);
        Task<IEnumerable<ProductDto>> GetFrequentlyBoughtTogetherAsync(int productId, int count = 5);
    }
}

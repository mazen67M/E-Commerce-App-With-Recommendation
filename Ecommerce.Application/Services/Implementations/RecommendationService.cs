using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class RecommendationService : IRecommendationService
    {
        public Task<IEnumerable<ProductDto>> GetPersonalizedRecommendationsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetRecommendationsAsync(string userId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetRelatedProductsAsync(int productId)
        {
            throw new NotImplementedException();
        }
    }
}

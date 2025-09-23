using Ecommerce.Application.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId);
        Task<double> GetProductAverageRatingAsync(int productId);
        Task AddReviewAsync(AddReviewDto reviewDto);
        Task<bool> CanUserReviewProductAsync(string userId, int productId);
    }
}

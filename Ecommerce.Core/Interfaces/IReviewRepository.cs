using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        /// Gets all reviews for a specific product.
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);

        /// Calculates the average rating for a specific product.
        Task<double> GetAverageRatingForProductAsync(int productId);

        /// Gets all reviews written by a specific user.
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId);

        /// Checks if a user has already reviewed a specific product
        Task<bool> HasUserReviewedProductAsync(string userId, int productId);

        /// Gets reviews with a specific rating.
        Task<IEnumerable<Review>> GetReviewsByRatingAsync(int rating);

        /// Gets the N most recent reviews.
        Task<IEnumerable<Review>> GetRecentReviewsAsync(int count);
    }
}

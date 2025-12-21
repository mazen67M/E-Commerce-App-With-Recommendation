using Ecommerce.Application.DTOs.Review;
using Ecommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecoomerce.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;
        private readonly IActivityLogService _activityLogService;

        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger, IActivityLogService activityLogService)
        {
            _reviewService = reviewService;
            _logger = logger;
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(int productId, int page = 1, int pageSize = 10)
        {
            try
            {
                var reviews = await _reviewService.GetProductReviewsAsync(productId);
                var averageRating = await _reviewService.GetProductAverageRatingAsync(productId);
                
                return Json(new
                {
                    success = true,
                    reviews = reviews,
                    averageRating = averageRating,
                    totalCount = reviews.Count()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for product {ProductId}", productId);
                return Json(new { success = false, message = "Failed to load reviews" });
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int productId, int rating, string comment)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                // Check if user can review
                var canReview = await _reviewService.CanUserReviewProductAsync(userId, productId);
                if (!canReview)
                {
                    return Json(new { success = false, message = "You have already reviewed this product or haven't purchased it" });
                }

                var reviewDto = new AddReviewDto
                {
                    ProductID = productId,
                    UserID = userId,
                    Rating = rating,
                    Comment = comment
                };

                await _reviewService.AddReviewAsync(reviewDto);
                
                // Log review activity
                await _activityLogService.LogActivityAsync(
                    userId,
                    "ReviewAdded",
                    "Product",
                    productId,
                    $"Added review with {rating} stars: {comment?.Substring(0, Math.Min(comment.Length, 50))}..."
                );
                
                return Json(new { success = true, message = "Review added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding review");
                return Json(new { success = false, message = "Failed to add review" });
            }
        }
    }
}

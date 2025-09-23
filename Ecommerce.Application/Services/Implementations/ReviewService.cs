using AutoMapper;
using Ecommerce.Application.DTOs.Review;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IReviewRepository reviewRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        // Done
        public async Task AddReviewAsync(AddReviewDto reviewDto)
        {
            if (!await CanUserReviewProductAsync(reviewDto.UserID, reviewDto.ProductID))
            {
                throw new InvalidOperationException("User cannot review this product.");
            }

            var review = _mapper.Map<Review>(reviewDto);
            await _reviewRepository.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        // Done
        public async Task<bool> CanUserReviewProductAsync(string userId, int productId)
        {
            var hasReviewed = await _reviewRepository.HasUserReviewedProductAsync(userId,productId);
            if (hasReviewed)
                return false;

            var userOrders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return userOrders.Any(order =>
                                    order.Status == Core.Enums.OrderStatus.delivered &&
                                    order.OrderItems.Any(item => 
                                                         item.ProductID == productId));
        }

        // Done
        public async Task<double> GetProductAverageRatingAsync(int productId)
        {
            return await _reviewRepository.GetAverageRatingForProductAsync(productId);
        }

        // Done
        public async Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }
    }
}

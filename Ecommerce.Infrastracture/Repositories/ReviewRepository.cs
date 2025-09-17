using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Done
        public async Task<double> GetAverageRatingForProductAsync(int productId)
        {
            var average = await _context.Reviews
               .Where(r => r.ProductID == productId)
               .AverageAsync(r => (double?)r.Rating); // Cast to double? for EF Core

            return average ?? 0.0;
        }

        // Done
        public async Task<IEnumerable<Review>> GetRecentReviewsAsync(int count)
        {
            return await _context.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .Include(r => r.User)
                .Include(r => r.Product)
                .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                            .Where(r => r.ProductID == productId)
                            .Include(r => r.User) // Include user info if needed in DTO
                            .ToListAsync();
        }

        // Done
        public async Task<IEnumerable<Review>> GetReviewsByRatingAsync(int rating)
        {
            return await _context.Reviews
                     .Where(r => r.Rating == rating)
                     .ToListAsync();
        }
        
        // Done
        public  async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId)
        {
            return await _context.Reviews
                            .Where(r => r.UserID == userId)
                            .Include(r => r.Product) // Include product info if needed in DTO
                            .ToListAsync();
        }
        
        // Done
        public async Task<bool> HasUserReviewedProductAsync(string userId, int productId)
        {
            return await _context.Reviews
                            .AnyAsync(r => r.UserID == userId && r.ProductID == productId);
        }
    }
}

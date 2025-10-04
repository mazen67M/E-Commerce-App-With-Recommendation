using Ecommerce.Application.DTOs.Reporting;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Interfaces;

namespace Ecommerce.Application.Services.Implementations
{
    public class UserReportService : IUserReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserSummaryDto> GetUserSummaryAsync()
        {
            var users = await _unitOfWork.Users.ListAllAsync();
            var usersWithOrders = await _unitOfWork.Users.GetUsersWithOrdersAsync();
            var usersWithoutOrders = await _unitOfWork.Users.GetUsersWithoutOrdersAsync();

            var thisMonth = DateTime.UtcNow.AddDays(-30);
            var newUsersThisMonth = users.Where(u => u.CreatedAt >= thisMonth).Count();

            var registrationTrend = await GetRegistrationTrendAsync(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);

            var totalOrders = (await _unitOfWork.Orders.ListAllAsync()).Count();
            var averageOrdersPerUser = users.Any() ? (decimal)totalOrders / users.Count() : 0;

            return new UserSummaryDto
            {
                TotalUsers = users.Count(),
                NewUsersThisMonth = newUsersThisMonth,
                ActiveUsers = usersWithOrders.Count(),
                UsersWithOrders = usersWithOrders.Count(),
                UsersWithoutOrders = usersWithoutOrders.Count(),
                AverageOrdersPerUser = averageOrdersPerUser,
                RegistrationTrend = registrationTrend.ToList()
            };
        }

        public async Task<IEnumerable<UserReportDto>> GetUserReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var users = startDate.HasValue || endDate.HasValue
                ? await _unitOfWork.Users.GetUsersRegisteredBetweenAsync(
                    startDate ?? DateTime.MinValue, 
                    endDate ?? DateTime.MaxValue)
                : await _unitOfWork.Users.ListAllAsync();

            var allOrders = await _unitOfWork.Orders.ListAllAsync();

            return users.Select(u => 
            {
                var userOrders = allOrders.Where(o => o.UserID == u.Id);
                return new UserReportDto
                {
                    UserId = u.Id,
                    Email = u.Email ?? "N/A",
                    FirstName = u.FirstName ?? "N/A",
                    LastName = u.LastName ?? "N/A",
                    RegistrationDate = u.CreatedAt,
                    TotalOrders = userOrders.Count(),
                    TotalSpent = userOrders.Sum(o => o.TotalAmount),
                    LastOrderDate = userOrders.Any() ? userOrders.Max(o => o.OrderDate) : null,
                    IsActive = userOrders.Any(o => o.OrderDate >= DateTime.UtcNow.AddDays(-90))
                };
            }).OrderByDescending(u => u.RegistrationDate);
        }

        public async Task<IEnumerable<UserActivityDto>> GetUserActivityReportAsync()
        {
            var users = await _unitOfWork.Users.ListAllAsync();

            return users.Select(u => new UserActivityDto
            {
                UserId = u.Id,
                Email = u.Email ?? "N/A",
                LastLoginDate = u.CreatedAt, // Using CreatedAt as LastLoginDate is not available
                CartItemsCount = u.Cart?.Items?.Count ?? 0,
                WishlistItemsCount = u.Wishlist?.Items?.Count ?? 0,
                ReviewsCount = u.Reviews?.Count ?? 0
            }).OrderByDescending(u => u.LastLoginDate);
        }

        public async Task<IEnumerable<UserRegistrationTrendDto>> GetRegistrationTrendAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var start = startDate ?? DateTime.UtcNow.AddDays(-30);
            var end = endDate ?? DateTime.UtcNow;

            var users = await _unitOfWork.Users.GetUsersRegisteredBetweenAsync(start, end);

            return users
                .GroupBy(u => u.CreatedAt.Date)
                .Select(g => new UserRegistrationTrendDto
                {
                    Date = g.Key,
                    NewRegistrations = g.Count()
                })
                .OrderBy(t => t.Date);
        }

        public async Task<IEnumerable<UserReportDto>> GetTopCustomersAsync(int count = 10)
        {
            var users = await _unitOfWork.Users.ListAllAsync();
            var allOrders = await _unitOfWork.Orders.ListAllAsync();

            var topCustomers = users.Select(u => 
            {
                var userOrders = allOrders.Where(o => o.UserID == u.Id);
                return new UserReportDto
                {
                    UserId = u.Id,
                    Email = u.Email ?? "N/A",
                    FirstName = u.FirstName ?? "N/A",
                    LastName = u.LastName ?? "N/A",
                    RegistrationDate = u.CreatedAt,
                    TotalOrders = userOrders.Count(),
                    TotalSpent = userOrders.Sum(o => o.TotalAmount),
                    LastOrderDate = userOrders.Any() ? userOrders.Max(o => o.OrderDate) : null,
                    IsActive = userOrders.Any(o => o.OrderDate >= DateTime.UtcNow.AddDays(-90))
                };
            })
            .Where(u => u.TotalOrders > 0)
            .OrderByDescending(u => u.TotalSpent)
            .Take(count);

            return topCustomers;
        }
    }
}

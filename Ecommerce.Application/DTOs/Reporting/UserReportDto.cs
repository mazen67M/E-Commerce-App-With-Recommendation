namespace Ecommerce.Application.DTOs.Reporting
{
    public class UserReportDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserActivityDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int CartItemsCount { get; set; }
        public int WishlistItemsCount { get; set; }
        public int ReviewsCount { get; set; }
    }

    public class UserSummaryDto
    {
        public int TotalUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int ActiveUsers { get; set; }
        public int UsersWithOrders { get; set; }
        public int UsersWithoutOrders { get; set; }
        public decimal AverageOrdersPerUser { get; set; }
        public List<UserRegistrationTrendDto> RegistrationTrend { get; set; } = new();
    }

    public class UserRegistrationTrendDto
    {
        public DateTime Date { get; set; }
        public int NewRegistrations { get; set; }
    }
}

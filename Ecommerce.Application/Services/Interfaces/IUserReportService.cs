using Ecommerce.Application.DTOs.Reporting;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IUserReportService
    {
        Task<UserSummaryDto> GetUserSummaryAsync();
        Task<IEnumerable<UserReportDto>> GetUserReportAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<UserActivityDto>> GetUserActivityReportAsync();
        Task<IEnumerable<UserRegistrationTrendDto>> GetRegistrationTrendAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<UserReportDto>> GetTopCustomersAsync(int count = 10);
    }
}

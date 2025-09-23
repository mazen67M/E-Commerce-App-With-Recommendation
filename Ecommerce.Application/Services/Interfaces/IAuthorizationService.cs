using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> IsAdminAsync(string userId);
        Task<bool> CanUserAccessOrderAsync(string userId, int orderId);
    }
}
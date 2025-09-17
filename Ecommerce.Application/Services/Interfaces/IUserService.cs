using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserProfileAsync(string userId);
        Task UpdateUserProfileAsync(string userId,UserDto userDto);
        Task<IEnumerable<OrderDto>> GetUserHistoryAsync(string userId);
        Task<decimal> GetUserTotalSpentAsync(string userId);
        Task<bool> IsUserAdminAsync(string userId);
        Task<IEnumerable<OrderDto>> GetUserOrderHistoryAsync(string userId);
    }
}

using AutoMapper;
using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetUserHistoryAsync(string userId)
        {
            return await GetUserOrderHistoryAsync(userId);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrderHistoryAsync(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(o => new OrderDto
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.Status,
                PaymentMethod = o.PaymentMethod,
                ShippingAddress = o.ShippingAddress,
                TotalAmount = o.TotalAmount,
                ItemsCount = o.OrderItems.Count
            });
        }

        public async Task<UserDto> GetUserProfileAsync(string userId)
        {
            var user = await _userRepository.GetUserWithCartAsync(userId);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AddressLine1 = user.AddressLine1,
                Country = user.Country,
                ImageUrl = user.ImageUrl,
                CreatedAt = user.CreatedAt
            };
        }

        public Task<decimal> GetUserTotalSpentAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserAdminAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserProfileAsync(string userId, UpdateUserDto userDto)
        {
            var user = await _userRepository.GetUserWithCartAsync(userId);
            if (user != null)
            {
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.PhoneNumber = userDto.PhoneNumber;
                user.AddressLine1 = userDto.AddressLine1;
                user.Country = userDto.Country;
                user.ImageUrl = userDto.ImageUrl;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}

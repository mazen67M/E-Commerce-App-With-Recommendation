using Ecommerce.Application.DTOs.User;
using System.Collections.Generic;

namespace Ecommerce.Application.ViewModels.Admin_Panel
{
    public class UserListViewModel
    {
        public List<UserDto> Users { get; set; } = new();
    }
}
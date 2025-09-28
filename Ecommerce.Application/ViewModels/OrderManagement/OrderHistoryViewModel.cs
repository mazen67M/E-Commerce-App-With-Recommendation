using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.OrderManagement
{
    public class OrderHistoryViewModel
    {
        public List<OrderDto> Orders { get; set; } = new();
    }

}

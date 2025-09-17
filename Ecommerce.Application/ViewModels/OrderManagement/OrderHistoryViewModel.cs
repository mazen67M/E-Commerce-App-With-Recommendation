using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.OrderManagement
{
    public class OrderHistoryViewModel
    {
        public List<OrderSummaryDto> Orders { get; set; } = new();
    }

}

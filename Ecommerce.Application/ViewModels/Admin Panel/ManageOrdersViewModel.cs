using Ecommerce.Application.DTOs;
using Ecommerce.Application.DTOs.Order;
using Ecommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ViewModels.Admin_Panel
{
    public class ManageOrdersViewModel
    {
        public List<OrderDetailsDto> Orders { get; set; } = new();
        public OrderStatus? FilterStatus { get; set; }
        public string SearchUser { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

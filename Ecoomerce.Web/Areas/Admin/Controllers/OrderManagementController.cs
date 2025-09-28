using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels.Admin_Panel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderManagementController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderManagementController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Displays a list of all orders
        public async Task<IActionResult> Index()
        {
            // Note: You would typically add a method to your IOrderService like GetAllOrdersAsync()
            // which the Admin role can access. For now, we'll imagine it exists.
            // var allOrders = await _orderService.GetAllOrdersAsync(); 
            var viewModel = new ManageOrdersViewModel
            {
                // Orders = allOrders.ToList()
            };
            return View(viewModel);
        }

        // Displays the details of a single order
        public async Task<IActionResult> Details(int orderId)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(orderId);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }
    }
}
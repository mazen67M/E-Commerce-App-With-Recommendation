using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels.Admin_Panel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")] // Specifies that this controller is part of the "Admin" area
    [Authorize(Roles = "Admin")] // Secures the entire controller for users in the "Admin" role
    public class DashboardController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public DashboardController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new AdminDashboardViewModel
            {
                // You would fetch real stats here
                TotalProducts = (await _productService.GetAllProductsAsync()).Count(),
            };
            return View(viewModel);
        }
    }
}
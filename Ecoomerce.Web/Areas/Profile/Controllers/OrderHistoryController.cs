using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels.OrderManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IAuthorizationService = Ecommerce.Application.Services.Interfaces.IAuthorizationService;

namespace Ecommerce.Web.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Authorize]
    public class OrderHistoryController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAuthorizationService _authorizationService;

        public OrderHistoryController(IOrderService orderService, IAuthorizationService authorizationService)
        {
            _orderService = orderService;
            _authorizationService = authorizationService;
        }

        // Action to show the list of past orders
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetUserOrdersAsync(userId);
            var viewModel = new OrderHistoryViewModel
            {
                Orders = orders.ToList()
            };
            return View(viewModel);
        }

        // Action to show details of a single past order
        public async Task<IActionResult> Details(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!await _authorizationService.CanUserAccessOrderAsync(userId, orderId))
            {
                return Forbid(); // Or return RedirectToAction("AccessDenied", "Account", new { area = "" });
            }

            var orderDetails = await _orderService.GetOrderDetailsAsync(orderId);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }
    }
}
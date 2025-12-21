using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.DTOs.Order;
using Ecommerce.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Ecoomerce.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ILogger<CheckoutController> _logger;
        private readonly IActivityLogService _activityLogService;

        public CheckoutController(
            ICartService cartService,
            IProductService productService,
            IOrderService orderService,
            ILogger<CheckoutController> logger,
            IActivityLogService activityLogService)
        {
            _cartService = cartService;
            _productService = productService;
            _orderService = orderService;
            _logger = logger;
            _activityLogService = activityLogService;
        }

        // Step 1: Cart Summary
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var cart = await _cartService.GetOrCreateCartAsync(userId);

                // Populate product details for each cart item
                foreach (var item in cart.Items)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductID);
                    if (product != null)
                    {
                        item.ProductName = product.Name;
                        item.ImageURL = product.ImageURL;
                        item.UnitPrice = product.Price;
                    }
                }

                var viewModel = new CheckoutViewModel
                {
                    Cart = new CartViewModel
                    {
                        Items = cart.Items,
                        Tax = cart.Tax,
                        Shipping = cart.Shipping,
                        Discount = cart.Discount
                    }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart for checkout");
                TempData["Error"] = "Failed to load checkout. Please try again.";
                return RedirectToAction("Index", "Cart");
            }
        }

        // Step 2: Shipping Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShippingInfo(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Store cart info in TempData
            TempData["CartItems"] = System.Text.Json.JsonSerializer.Serialize(model.Cart.Items);
            TempData["CartSubTotal"] = model.Cart.SubTotal;
            TempData["CartTax"] = model.Cart.Tax;
            TempData["CartShipping"] = model.Cart.Shipping;
            TempData["CartDiscount"] = model.Cart.Discount;

            return View(model);
        }

        // Step 3: Payment Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PaymentMethod(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ShippingInfo", model);
            }

            // Store shipping info in TempData
            TempData["FullName"] = model.FullName;
            TempData["Email"] = model.Email;
            TempData["Phone"] = model.Phone;
            TempData["ShippingAddress"] = model.ShippingAddress;
            TempData["City"] = model.City;
            TempData["State"] = model.State;
            TempData["ZipCode"] = model.ZipCode;
            TempData["Country"] = model.Country;
            TempData["UseShippingAsBilling"] = model.UseShippingAsBilling;

            if (!model.UseShippingAsBilling)
            {
                TempData["BillingAddress"] = model.BillingAddress;
            }

            // Restore cart info from TempData
            if (TempData["CartItems"] != null)
            {
                model.Cart.Items = JsonSerializer.Deserialize<List<CartItemDto>>(TempData["CartItems"].ToString());
                TempData.Keep("CartItems");
            }

            // SubTotal is a calculated property
            TempData.Keep("CartSubTotal");

            if (TempData["CartTax"] != null)
            {
                model.Cart.Tax = Convert.ToDecimal(TempData["CartTax"]);
                TempData.Keep("CartTax");
            }

            if (TempData["CartShipping"] != null)
            {
                model.Cart.Shipping = Convert.ToDecimal(TempData["CartShipping"]);
                TempData.Keep("CartShipping");
            }

            if (TempData["CartDiscount"] != null)
            {
                model.Cart.Discount = Convert.ToDecimal(TempData["CartDiscount"]);
                TempData.Keep("CartDiscount");
            }

            return View(model);
        }

        // Step 4: Order Confirmation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderConfirmation(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("PaymentMethod", model);
            }

            // Store payment info in TempData
            TempData["PaymentMethod"] = model.PaymentMethod;

            if (model.PaymentMethod == "creditCard")
            {
                // Store masked card information for security
                if (!string.IsNullOrEmpty(model.CardNumber) && model.CardNumber.Length >= 4)
                {
                    TempData["CardLastFour"] = model.CardNumber.Substring(model.CardNumber.Length - 4);
                }
                TempData["CardExpiry"] = model.CardExpiry;
            }

            // Restore shipping info from TempData
            model.FullName = TempData["FullName"]?.ToString();
            model.Email = TempData["Email"]?.ToString();
            model.Phone = TempData["Phone"]?.ToString();
            model.ShippingAddress = TempData["ShippingAddress"]?.ToString();
            model.City = TempData["City"]?.ToString();
            model.State = TempData["State"]?.ToString();
            model.ZipCode = TempData["ZipCode"]?.ToString();
            model.Country = TempData["Country"]?.ToString();
            model.UseShippingAsBilling = Convert.ToBoolean(TempData["UseShippingAsBilling"]);

            if (!model.UseShippingAsBilling)
            {
                model.BillingAddress = TempData["BillingAddress"]?.ToString();
            }

            // Restore cart info from TempData
            if (TempData["CartItems"] != null)
            {
                model.Cart.Items = JsonSerializer.Deserialize<List<CartItemDto>>(TempData["CartItems"].ToString());
                TempData.Keep("CartItems");
            }

            // SubTotal is a calculated property
            TempData.Keep("CartSubTotal");

            if (TempData["CartTax"] != null)
            {
                model.Cart.Tax = Convert.ToDecimal(TempData["CartTax"]);
                TempData.Keep("CartTax");
            }

            if (TempData["CartShipping"] != null)
            {
                model.Cart.Shipping = Convert.ToDecimal(TempData["CartShipping"]);
                TempData.Keep("CartShipping");
            }

            if (TempData["CartDiscount"] != null)
            {
                model.Cart.Discount = Convert.ToDecimal(TempData["CartDiscount"]);
                TempData.Keep("CartDiscount");
            }

            // Keep TempData for the next request
            TempData.Keep();

            return View(model);
        }

        // Step 5: Process Order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                // Create order using the service
                var shippingAddress = TempData["ShippingAddress"]?.ToString() ?? string.Empty;
                var paymentMethod = TempData["PaymentMethod"]?.ToString() ?? "CreditCard";
                
                var order = await _orderService.CreateOrderAsync(userId, shippingAddress, paymentMethod);
                
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Failed to create order. Please try again.";
                    return RedirectToAction("OrderConfirmation");
                }
                
                // Log order creation activity
                await _activityLogService.LogActivityAsync(
                    userId,
                    "OrderCreated",
                    "Order",
                    order.OrderID,
                    $"Order created - Total: {order.TotalAmount:C} L.E"
                );
                
                // Clear the cart
                await _cartService.ClearCartAsync(userId);
                
                // Clear TempData
                TempData.Clear();

                return RedirectToAction("OrderComplete", new { id = order.OrderID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing order");
                TempData["Error"] = "Failed to process your order. Please try again.";
                return RedirectToAction("OrderConfirmation");
            }
        }

        // Step 6: Order Complete
        public async Task<IActionResult> OrderComplete(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var orderDetails = await _orderService.GetOrderDetailsAsync(id);

                if (orderDetails == null)
                {
                    return NotFound();
                }

                // Return the DTO directly - the view uses OrderDetailsViewModel which matches the structure
                return View(orderDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details");
                TempData["Error"] = "Failed to load order details. Please check your order history.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
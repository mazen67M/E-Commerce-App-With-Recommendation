using Ecommerce.Application.ViewModels.Profile;
using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecoomerce.Web.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Authorize]
    public class AddressController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddressController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profile/Address
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var addresses = await _context.UserAddresses
                .Where(a => a.UserId == userId && a.IsActive)
                .OrderByDescending(a => a.IsDefaultShipping)
                .ThenByDescending(a => a.IsDefaultBilling)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();

            var viewModel = new AddressListViewModel
            {
                Addresses = addresses.Select(a => new AddressViewModel
                {
                    AddressId = a.AddressId,
                    AddressName = a.AddressName,
                    RecipientName = a.RecipientName,
                    Phone = a.Phone,
                    AddressLine1 = a.AddressLine1,
                    AddressLine2 = a.AddressLine2,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                    Country = a.Country,
                    IsDefaultShipping = a.IsDefaultShipping,
                    IsDefaultBilling = a.IsDefaultBilling
                }).ToList(),
                DefaultShipping = addresses.FirstOrDefault(a => a.IsDefaultShipping) != null 
                    ? MapToViewModel(addresses.First(a => a.IsDefaultShipping)) : null,
                DefaultBilling = addresses.FirstOrDefault(a => a.IsDefaultBilling) != null 
                    ? MapToViewModel(addresses.First(a => a.IsDefaultBilling)) : null
            };

            return View(viewModel);
        }

        // GET: Profile/Address/Create
        public IActionResult Create()
        {
            return View(new AddressViewModel());
        }

        // POST: Profile/Address/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            // If setting as default, clear other defaults
            if (model.IsDefaultShipping)
            {
                await ClearDefaultShipping(userId);
            }
            if (model.IsDefaultBilling)
            {
                await ClearDefaultBilling(userId);
            }

            var address = new UserAddress
            {
                UserId = userId,
                AddressName = model.AddressName,
                RecipientName = model.RecipientName,
                Phone = model.Phone,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Country = model.Country,
                IsDefaultShipping = model.IsDefaultShipping,
                IsDefaultBilling = model.IsDefaultBilling
            };

            _context.UserAddresses.Add(address);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Address added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Profile/Address/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound();
            }

            return View(MapToViewModel(address));
        }

        // POST: Profile/Address/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddressViewModel model)
        {
            if (id != model.AddressId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound();
            }

            // If setting as default, clear other defaults
            if (model.IsDefaultShipping && !address.IsDefaultShipping)
            {
                await ClearDefaultShipping(userId);
            }
            if (model.IsDefaultBilling && !address.IsDefaultBilling)
            {
                await ClearDefaultBilling(userId);
            }

            address.AddressName = model.AddressName;
            address.RecipientName = model.RecipientName;
            address.Phone = model.Phone;
            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.City = model.City;
            address.State = model.State;
            address.PostalCode = model.PostalCode;
            address.Country = model.Country;
            address.IsDefaultShipping = model.IsDefaultShipping;
            address.IsDefaultBilling = model.IsDefaultBilling;
            address.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Address updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Profile/Address/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound();
            }

            // Soft delete
            address.IsActive = false;
            address.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Address deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Profile/Address/SetDefault
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetDefault(int id, string type)
        {
            var userId = _userManager.GetUserId(User);
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound();
            }

            if (type == "shipping")
            {
                await ClearDefaultShipping(userId);
                address.IsDefaultShipping = true;
                TempData["Success"] = "Default shipping address updated!";
            }
            else if (type == "billing")
            {
                await ClearDefaultBilling(userId);
                address.IsDefaultBilling = true;
                TempData["Success"] = "Default billing address updated!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task ClearDefaultShipping(string userId)
        {
            var addresses = await _context.UserAddresses
                .Where(a => a.UserId == userId && a.IsDefaultShipping)
                .ToListAsync();
            foreach (var a in addresses)
            {
                a.IsDefaultShipping = false;
            }
        }

        private async Task ClearDefaultBilling(string userId)
        {
            var addresses = await _context.UserAddresses
                .Where(a => a.UserId == userId && a.IsDefaultBilling)
                .ToListAsync();
            foreach (var a in addresses)
            {
                a.IsDefaultBilling = false;
            }
        }

        private AddressViewModel MapToViewModel(UserAddress address)
        {
            return new AddressViewModel
            {
                AddressId = address.AddressId,
                AddressName = address.AddressName,
                RecipientName = address.RecipientName,
                Phone = address.Phone,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                IsDefaultShipping = address.IsDefaultShipping,
                IsDefaultBilling = address.IsDefaultBilling
            };
        }
    }
}

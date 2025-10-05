using AutoMapper;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels.Admin_Panel;
using Ecommerce.Application.ViewModels.User___Account;
using Ecommerce.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommerce.Web.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Authorize]
    public class ManageProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;

        public ManageProfileController(IUserService userService, IMapper mapper, IFileUploadService fileUploadService)
        {
            _userService = userService;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDto = await _userService.GetUserProfileAsync(userId);
            if (userDto == null) return NotFound();

            var viewModel = _mapper.Map<EditProfileViewModel>(userDto);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDto = _mapper.Map<UpdateUserDto>(model);
            userDto.Id = userId; 

            await _userService.UpdateUserProfileAsync(userId, userDto);

            TempData["SuccessMessage"] = "Your profile has been updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select an image file.";
                return RedirectToAction("Index");
            }

            if (!_fileUploadService.IsValidImage(profilePicture))
            {
                TempData["ErrorMessage"] = "Invalid image file. Please upload a JPG, PNG, or GIF file (max 5MB).";
                return RedirectToAction("Index");
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userDto = await _userService.GetUserProfileAsync(userId);

                // Delete old image if exists
                if (!string.IsNullOrEmpty(userDto.ImageUrl))
                {
                    await _fileUploadService.DeleteImageAsync(userDto.ImageUrl);
                }

                // Upload new image
                var imageUrl = await _fileUploadService.UploadImageAsync(profilePicture, "profiles");

                // Update user profile
                var updateDto = new UpdateUserDto
                {
                    Id = userId,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    PhoneNumber = userDto.PhoneNumber,
                    AddressLine1 = userDto.AddressLine1,
                    Country = userDto.Country,
                    ImageUrl = imageUrl
                };

                await _userService.UpdateUserProfileAsync(userId, updateDto);

                TempData["SuccessMessage"] = "Profile picture updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to upload image: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
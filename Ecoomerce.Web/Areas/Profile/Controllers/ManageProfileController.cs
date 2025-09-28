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

        public ManageProfileController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
    }
}
using AutoMapper;
using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels.Forms___Input_Models;
using Ecommerce.Application.ViewModels.User___Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecoomerce.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAuthenticationService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(
            IAuthenticationService authService,
            IMapper mapper,
            ILogger<AccountController> logger,
            IUserService userService)
        {
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registerDto = _mapper.Map<RegisterViewModel>(model);
            var result = await _authService.RegisterAsync(registerDto);

            if (result.IsSuccess)
            {
                _logger.LogInformation($"New User Registed With Email: {model.Email}");
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }


        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            var loginDto = _mapper.Map<LoginDto>(model);
            var result = await _authService.LoginAsync(loginDto); 

            if (result.IsSuccess)
            {
                _logger.LogInformation($"User logged in: {model.Email}");
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return LocalRedirect(returnUrl ?? "/");
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name;
            await _authService.LogoutAsync();
            _logger.LogInformation($"User {userName} logged out.");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Porfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDto = await _userService.GetUserProfileAsync(userId!);

            if (userDto == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<ProfileViewModel>(userDto);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

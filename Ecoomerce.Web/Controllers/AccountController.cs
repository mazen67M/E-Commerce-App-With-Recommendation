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
        private readonly IEmailSenderService _emailSender;

        public AccountController(
            IAuthenticationService authService,
            IMapper mapper,
            ILogger<AccountController> logger,
            IUserService userService,
            IEmailSenderService emailSender)
        {
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _emailSender = emailSender;
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
                _logger.LogInformation($"New User Registered With Email: {model.Email}");

                // Generate email confirmation token
                var user = await _authService.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _authService.GenerateEmailConfirmationTokenAsync(user.Id);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    // Send confirmation email
                   var emailBody = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                            <h2 style='color: #28a745;'>Welcome to ECommerce App!</h2>
                            <p>Thank you for registering with us.</p>
                            <p>Please confirm your email address by clicking the button below:</p>
                            <a href='{confirmationLink}' 
                            style='display: inline-block; padding: 12px 24px; background-color: #28a745; 
                                    color: white; text-decoration: none; border-radius: 4px; margin: 20px 0;'>
                                Confirm My Email
                            </a>
                            <p style='color: #666; font-size: 12px;'>
                                If you didn't create an account, please ignore this email.
                            </p>
                        </div>
                    ";
                    await _emailSender.SendEmailAsync(model.Email, "Confirm Your Email", emailBody);

                    TempData["Message"] = "Registration successful! Please check your email to confirm your account.";
                }

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

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var result = await _authService.ConfirmEmailAsync(userId, token);
            if (result)
            {
                TempData["Message"] = "Email confirmed successfully! You can now log in.";
                return RedirectToAction("Login", "Account");
            }

            TempData["Error"] = "Email confirmation failed. Please try again or contact support.";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet] 
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var token = await _authService.GeneratePasswordResetTokenAsync(model.Email);
                var resetLink = Url.Action("ResetPassword", "Account",
                    new { email = model.Email, token = token }, Request.Scheme);

                // Send password reset email
                var emailBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #dc3545;'>Password Reset Request</h2>
                        <p>You requested to reset your password.</p>
                        <p>Please click the button below to reset your password:</p>
                        <a href='{resetLink}' 
                           style='display: inline-block; padding: 12px 24px; background-color: #dc3545; 
                                  color: white; text-decoration: none; border-radius: 4px; margin: 20px 0;'>
                            Reset Password
                        </a>
                        <p style='color: #666; font-size: 12px;'>
                            If you didn't request a password reset, please ignore this email.
                        </p>
                        <p style='color: #666; font-size: 12px;'>
                            This link will expire in 24 hours.
                        </p>
                    </div>
                ";

                await _emailSender.SendEmailAsync(model.Email, "Reset Your Password", emailBody);
                TempData["Message"] = "Password reset link has been sent to your email.";
                return RedirectToAction("Login");
            }
            catch (ArgumentException)
            {
                // Don't reveal that the user doesn't exist
                TempData["Message"] = "If your email exists in our system, you will receive a password reset link.";
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid password reset request.");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _authService.ResetPasswordAsync(model.Email, model.Token, model.Password);
                TempData["Message"] = "Your password has been reset successfully. You can now login with your new password.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to reset password. The link may have expired.");
                return View(model);
            }
        }
    }
}

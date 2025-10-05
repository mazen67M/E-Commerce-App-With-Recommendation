using AutoMapper;
using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.Services.Interfaces;
using Ecommerce.Application.ViewModels.Forms___Input_Models;
using Ecommerce.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly INotificationService _notificationService; 
        private readonly IMapper _mapper;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            INotificationService notificationService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterViewModel registerDto)
        {
            var user = _mapper.Map<ApplicationUser>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new AuthResultDto { IsSuccess = false, Message = string.Join("\n", result.Errors.Select(e => e.Description)) };
            }

            // Here you would typically generate an email confirmation token and send it
            // var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // await _notificationService.SendEmailConfirmationAsync(user.Email, token);

            return new AuthResultDto { IsSuccess = true, UserId = user.Id, Message = "Registration successful. Please check your email to confirm your account." };
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, Message = "Invalid email or password." };
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return new AuthResultDto { IsSuccess = true, UserId = user.Id, Email = user.Email, Message = "Login successful." };
            }

            string message = "Invalid email or password.";
            if (result.IsLockedOut) message = "This account has been locked out due to multiple failed login attempts. Please try again later.";
            if (result.IsNotAllowed) message = "You are not allowed to sign in. Please confirm your email first.";

            return new AuthResultDto { IsSuccess = false, Message = message };
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found", nameof(userId));
            }
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }


        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found", nameof(email));
            }
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }


        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found", nameof(email));
            }
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Password reset failed");
            }
        }

    }
}
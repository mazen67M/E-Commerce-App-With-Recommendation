using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.ViewModels.Forms___Input_Models;
using Ecommerce.Core.Entities;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResultDto> RegisterAsync(RegisterViewModel registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task LogoutAsync();
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
    }
}

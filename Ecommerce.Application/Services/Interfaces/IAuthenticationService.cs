using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.ViewModels.Forms___Input_Models;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResultDto> RegisterAsync(RegisterViewModel registerDto);
        Task<AuthResultDto> LoginAsync(LoginViewModel loginDto);
        Task LogoutAsync();
        Task<bool> ConfirmEmailAsync(string userId, string token);
    }
}

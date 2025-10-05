using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder);
        Task<bool> DeleteImageAsync(string imageUrl);
        bool IsValidImage(IFormFile file);
    }
}

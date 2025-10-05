using System;

namespace Ecommerce.Application.ViewModels.User___Account
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } // Optional
        public string? AddressLine1 { get; set; } // Optional
        public string? Country { get; set; } // Optional
        public string? ImageUrl { get; set; } // Profile picture
        public DateTime CreatedAt { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
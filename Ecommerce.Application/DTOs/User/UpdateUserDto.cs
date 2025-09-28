namespace Ecommerce.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public string Id { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public string? Country { get; set; }
    }
}
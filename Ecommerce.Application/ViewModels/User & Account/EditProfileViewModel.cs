using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.ViewModels.Admin_Panel
{
    public class EditProfileViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        // For capturing which roles are selected for the user
        public List<string> UserRoles { get; set; } = new();

        // For displaying all available roles
        public List<string> Roles { get; set; } = new();

    }
}
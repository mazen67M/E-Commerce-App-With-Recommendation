using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.ViewModels.Forms___Input_Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}

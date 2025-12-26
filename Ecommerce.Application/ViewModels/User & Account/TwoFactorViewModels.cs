using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.ViewModels.User___Account
{
    public class EnableTwoFactorViewModel
    {
        public bool IsTwoFactorEnabled { get; set; }
        public string? SharedKey { get; set; }
        public string? AuthenticatorUri { get; set; }
    }

    public class VerifyTwoFactorViewModel
    {
        [Required(ErrorMessage = "Verification code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Code must be 6 digits")]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }

        public bool RememberDevice { get; set; }
    }

    public class TwoFactorSettingsViewModel
    {
        public bool IsTwoFactorEnabled { get; set; }
        public bool HasAuthenticator { get; set; }
        public int RecoveryCodesLeft { get; set; }
    }
}

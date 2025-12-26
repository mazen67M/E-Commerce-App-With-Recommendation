using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.ViewModels.Profile
{
    public class AddressViewModel
    {
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Address name is required")]
        [MaxLength(100)]
        [Display(Name = "Address Label")]
        public string AddressName { get; set; } = "Home";

        [Required(ErrorMessage = "Recipient name is required")]
        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string RecipientName { get; set; }

        [Phone]
        [MaxLength(20)]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200)]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [MaxLength(200)]
        [Display(Name = "Address Line 2 (Optional)")]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        [Display(Name = "State/Province")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [MaxLength(20)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(100)]
        public string Country { get; set; } = "Egypt";

        [Display(Name = "Default Shipping Address")]
        public bool IsDefaultShipping { get; set; }

        [Display(Name = "Default Billing Address")]
        public bool IsDefaultBilling { get; set; }
    }

    public class AddressListViewModel
    {
        public List<AddressViewModel> Addresses { get; set; } = new();
        public AddressViewModel? DefaultShipping { get; set; }
        public AddressViewModel? DefaultBilling { get; set; }
    }
}

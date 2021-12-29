using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "Stores")]
    public class StoreModel : EntityModel
    {  
        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string StoreName { get; set; } = string.Empty;

        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string StoreNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Phone number is invalid.")]
        public string PhonePrimary { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Phone number is invalid.")]
        public string PhoneSecondary { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Phone number is invalid.")]
        public string PhoneMobile { get; set; } = string.Empty;

        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string StreetAddress1 { get; set; } = string.Empty;

        public string StreetAddress2 { get; set; } = string.Empty;

        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string City { get; set; } = string.Empty;

        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string State { get; set; } = string.Empty;

        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string CountryCode { get; set; } = string.Empty;

        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string PostalCode { get; set; } = string.Empty;

        [Url(ErrorMessage = "{0} is invalid.")]
        public string ProfilePicUrl { get; set; } = string.Empty;

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public bool IsSignatureRequired { get; set; }

        [ForeignKey(nameof(ServiceProviderModel))]
        public Guid ServiceProviderId { get; set; }

        public ServiceProviderModel? ServiceProvider { get; set; }

        public List<AssetModel>? Assets { get; set; }

        public IList<DriverStore>? DriverStores { get; set; }
    }
}

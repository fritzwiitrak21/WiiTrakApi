using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "Corporations")]
    public class CorporationModel: EntityModel
    {
        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string StreetAddress1 { get; set; } = string.Empty;

        [MaxLength(128)]
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

        [Url(ErrorMessage = "{0} is invalid.")]
        public string CompanyLogoUrl { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Phone number is invalid.")]
        public string PhonePrimary { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Phone number is invalid.")]
        public string PhoneSecondary { get; set; } = string.Empty;

        [ForeignKey(nameof(CompanyModel))]
        public Guid CompanyId { get; set; }

        public CompanyModel? Company { get; set; }

        public List<StoreModel> Stores { get; set; }

    }
}

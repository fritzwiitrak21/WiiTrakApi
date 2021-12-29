using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "ServiceProviders")]
    public class ServiceProviderModel : EntityModel
    {
        [MaxLength(128)]
        [Required(ErrorMessage = "{0} is required.")]
        public string ServiceProviderName { get; set; } =   string.Empty;

        [EmailAddress(ErrorMessage = "Email is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Phone number is invalid.")]
        public string PhonePrimary { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Phone number is invalid.")]
        public string PhoneSecondary { get; set; } = string.Empty;

        [Url(ErrorMessage = "{0} is invalid.")]
        public string LogoPicUrl { get; set; } = string.Empty;

        [ForeignKey(nameof(CompanyModel))]
        public Guid CompanyId { get; set; }

        public CompanyModel? Company { get; set; }

        public List<StoreModel>? Stores { get; set; }

    }
}

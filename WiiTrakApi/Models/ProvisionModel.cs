using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "Provisions")]
    public class ProvisionModel : EntityModel
    {
        // [MaxLength(128)]
        // [Required(ErrorMessage = "{0} is required.")]
        // [Phone(ErrorMessage = "Phone number is invalid.")]
        // [EmailAddress(ErrorMessage = "Email is invalid.")]
        // [Url(ErrorMessage = "{0} is invalid.")]
    }
}

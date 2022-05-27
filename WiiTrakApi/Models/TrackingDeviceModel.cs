using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    public class TrackingDeviceModel: EntityModel
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string DeviceName { get; set; } = string.Empty;

        public string Manufactor { get; set; } = string.Empty;

        public DateTime ManufacturedDate { get; set; }

        public DateTime InstalledDate { get; set; }

        public string TelecomCompanyName { get; set; } = string.Empty;

        public string SIMCardId { get; set; } = string.Empty;

        public string SIMCardPhoneNumber { get; set; } = string.Empty;

        public string IMEINumber { get; set; } = string.Empty;

        public string ModelNumber { get; set; } = string.Empty;
        

        [ForeignKey(nameof(CartModel))]
        public Guid CartId { get; set; }

        public CartModel? Cart { get; set; }


        [ForeignKey(nameof(SystemOwnerModel))]
        public Guid SystemOwnerId { get; set; }

        public SystemOwnerModel? SystemOwner { get; set; }
        public bool IsActive { get; set; }
    }
}

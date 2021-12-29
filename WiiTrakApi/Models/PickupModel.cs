using System.ComponentModel.DataAnnotations.Schema;
using WiiTrakApi.Enums;

namespace WiiTrakApi.Models
{
    [Table(name: "Pickups")]
    public class PickupModel : EntityModel
    {
        public Guid AssetId { get; set; }

        public DateTime PickedUpAt { get; set; }

        public DateTime DroppedOffAt { get; set; }           

        public Guid ServiceProviderId { get; set; } = Guid.Empty;

        public Guid StoreId { get; set; } = Guid.Empty;

        public Guid DriverId { get; set; } = Guid.Empty;

        public AssetCondition Condition { get; set; }

        public double PickupLongitude { get; set; }

        public double PickupLatitude { get; set; }
    }
}

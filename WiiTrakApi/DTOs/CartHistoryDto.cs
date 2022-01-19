using System.ComponentModel.DataAnnotations.Schema;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;

namespace WiiTrakApi.DTOs
{
    public class CartHistoryDto
    {
        public DateTime? PickedUpAt { get; set; }

        public DateTime? DroppedOffAt { get; set; }

        public DateTime? ProvisionedAt { get; set; }

        public Guid? ServiceProviderId { get; set; } = Guid.Empty;

        public Guid? StoreId { get; set; } = Guid.Empty;

        public Guid? DriverId { get; set; } = Guid.Empty;

        public CartCondition Condition { get; set; }

        public CartStatus Status { get; set; }

        public bool IsDelivered { get; set; }

        public double PickupLongitude { get; set; }

        public double PickupLatitude { get; set; }

        public Guid CartId { get; set; }
    }
}

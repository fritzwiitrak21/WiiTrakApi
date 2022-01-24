
using WiiTrakApi.Enums;

namespace WiiTrakApi.DTOs
{
    public class CartHistoryCreationDto
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

        public Guid? DeliveryTicketId { get; set; } = Guid.Empty;

        public double PickupLongitude { get; set; }

        public double PickupLatitude { get; set; }

        public Guid CartId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using WiiTrakApi.Enums;

namespace WiiTrakApi.DTOs
{
    public class DeliveryTicketCreationDto
    {
        public long DeliveryTicketNumber { get; set; }

        public int NumberOfCarts { get; set; }

        public string Grid { get; set; } = string.Empty;

        public string PicUrl { get; set; } = string.Empty;

        public DateTime DeliveredAt { get; set; }

        public Guid ServiceProviderId { get; set; } = Guid.Empty;

        public Guid StoreId { get; set; } = Guid.Empty;

        public Guid DriverId { get; set; } = Guid.Empty;
    }
}

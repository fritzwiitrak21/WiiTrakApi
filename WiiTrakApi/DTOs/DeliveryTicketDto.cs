using System.ComponentModel.DataAnnotations;
using WiiTrakApi.Enums;

namespace WiiTrakApi.DTOs
{
    public class DeliveryTicketDto
    {
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public long DeliveryTicketNumber { get; set; }

        public int NumberOfCarts { get; set; }

        public string Grid { get; set; } = string.Empty;

        public string PicUrl { get; set; } = string.Empty;

        public string SignaturePicUrl { get; set; } = string.Empty;

        public string StoreName { get; set; }

        public string StoreNumber { get; set; }

        public string DriverName { get; set; }

        public DateTime DeliveredAt { get; set; }

        public bool SignOffRequired { get; set; }

        public bool ApprovedByStore { get; set; }

        public Guid ServiceProviderId { get; set; } = Guid.Empty;

        public Guid StoreId { get; set; } = Guid.Empty;

        public Guid DriverId { get; set; } = Guid.Empty;

        public string Signee { get; set; } = string.Empty;

        public int DriverNumber { get; set; }
        //public bool IsActive { get; set; }
        //public Guid UpdatedBy { get;set; }
    }
}

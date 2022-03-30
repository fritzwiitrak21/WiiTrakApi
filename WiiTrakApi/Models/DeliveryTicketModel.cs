using System.ComponentModel.DataAnnotations;
using WiiTrakApi.Enums;

namespace WiiTrakApi.Models
{
   
    public class DeliveryTicketModel : EntityModel
    {
       
        public long DeliveryTicketNumber { get; set; }

        public int NumberOfCarts { get; set; }

        [MaxLength(128)]
        public string Grid { get; set; } = string.Empty;

        [Url(ErrorMessage = "{0} is invalid.")]
        public string PicUrl { get; set; } = string.Empty;

        [Url(ErrorMessage = "{0} is invalid.")]
        public string SignaturePicUrl { get; set; } = string.Empty;

        public bool SignOffRequired { get; set; }

        public bool ApprovedByStore { get; set; }

        public DateTime DeliveredAt { get; set; }

        public Guid ServiceProviderId { get; set; } = Guid.Empty;

        public Guid StoreId { get; set; } = Guid.Empty;

        public Guid DriverId { get; set; } = Guid.Empty;

        public string Signee { get; set; } = string.Empty;
    }
}

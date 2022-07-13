/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "DeliveryTickets")]
    public class DeliveryTicketModel : EntityModel
    {
       
        public long DeliveryTicketNumber { get; set; }
        public int NumberOfCarts { get; set; }

        public string Grid { get; set; } = string.Empty;

        public string PicUrl { get; set; } = string.Empty;
        public string SignaturePicUrl { get; set; } = string.Empty;

        public bool SignOffRequired { get; set; }

        public bool ApprovedByStore { get; set; }

        public DateTime DeliveredAt { get; set; }

        public Guid ServiceProviderId { get; set; } = Guid.Empty;

        public Guid StoreId { get; set; } = Guid.Empty;

        public Guid DriverId { get; set; } = Guid.Empty;
        public string Signee { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public Guid? UpdatedBy { get; set; } = Guid.Empty;

    }
}

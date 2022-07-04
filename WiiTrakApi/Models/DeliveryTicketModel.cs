/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations;

namespace WiiTrakApi.Models
{
   
    public class DeliveryTicketModel : EntityModel
    {
       
        public long DeliveryTicketNumber { get; set; }
        [Required]
        [Range(1, 300, ErrorMessage ="Value should be above or equal to 1")] 
        public int NumberOfCarts { get; set; }

        [MaxLength(128)]
        public string Grid { get; set; } = string.Empty;

        [Required]
        [Url(ErrorMessage = "{0} is invalid.")]
        public string PicUrl { get; set; } = string.Empty;
        [Required]
        [Url(ErrorMessage = "{0} is invalid.")]
        public string SignaturePicUrl { get; set; } = string.Empty;

        public bool SignOffRequired { get; set; }

        public bool ApprovedByStore { get; set; }

        public DateTime DeliveredAt { get; set; }

        public Guid ServiceProviderId { get; set; } = Guid.Empty;

        public Guid StoreId { get; set; } = Guid.Empty;

        public Guid DriverId { get; set; } = Guid.Empty;
        [Required]
        public string Signee { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public Guid? UpdatedBy { get; set; } = Guid.Empty;

    }
}

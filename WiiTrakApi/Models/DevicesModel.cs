/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace WiiTrakApi.Models
{
    [Table(name: "Devices")]
    public class DevicesModel: EntityModel
    {
        public string DeviceModel { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string IMEI { get; set; } = string.Empty;
        public string ICCID { get; set; } = string.Empty;
        public string IMSI { get; set; } = string.Empty;
        public Guid SIMCardId { get; set; }
        public string SIMNo { get; set; } = string.Empty;
        public DateTime ActivatedTime { get; set; }
        public DateTime SubscriptionExpiration { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid TechnicianId { get; set; }
        public bool IsMapped { get; set; }
    }
}

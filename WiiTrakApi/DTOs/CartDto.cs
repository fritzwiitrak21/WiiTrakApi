using System.ComponentModel.DataAnnotations.Schema;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;

namespace WiiTrakApi.DTOs
{  
    public record CartDto
    {
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CartNumber { get; set; } = string.Empty;

        public string ManufacturerName { get; set; } = string.Empty;

        public DateTime DateManufactured { get; set; }

        public CartOrderedFrom OrderedFrom { get; set; }

        public CartCondition Condition { get; set; }

        public CartStatus Status { get; set; }

        public string PicUrl { get; set; } = string.Empty;

        public bool IsProvisioned { get; set; }

        public string BarCode { get; set; } = string.Empty;

        public Guid StoreId { get; set; }

        public StoreDto? Store { get; set; }

        public TrackingDeviceDto? TrackingDevice { get; set; }

        public List<CartHistoryDto> CartHistory { get; set; }

        public Guid DeviceId { get; set; }
        public bool IsActive { get; set; }
    }
}

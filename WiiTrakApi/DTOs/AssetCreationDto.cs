using WiiTrakApi.Enums;
using WiiTrakApi.Models;

namespace WiiTrakApi.DTOs
{
    public record AssetCreationDto
    {
        public DateTime CreatedAt { get; set; }

        public string ManufacturerName { get; set; } = string.Empty;

        public DateTime DateManufactured { get; set; }

        public AssetOrderedFrom OrderedFrom { get; set; }

        public AssetCondition Condition { get; set; }

        public AssetStatus Status { get; set; }

        public string PicUrl { get; set; } = string.Empty;

        public bool IsProvisioned { get; set; }

        public string BarCode { get; set; } = string.Empty;

        public Guid StoreId { get; set; }
    }
}

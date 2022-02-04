using WiiTrakApi.Enums;
using WiiTrakApi.Models;

namespace WiiTrakApi.DTOs
{
    public record CartUpdateDto
    {
        public string ManufacturerName { get; set; } = string.Empty;

        public DateTime DateManufactured { get; set; }

        public string CartNumber { get; set; } = string.Empty;

        public CartOrderedFrom OrderedFrom { get; set; }

        public CartCondition Condition { get; set; }

        public CartStatus Status { get; set; }

        public string PicUrl { get; set; } = string.Empty;

        public bool IsProvisioned { get; set; }

        public string BarCode { get; set; } = string.Empty;

        public Guid StoreId { get; set; }

        public CartHistoryUpdateDto CartHistory { get; set; }
    }
}

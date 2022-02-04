using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WiiTrakApi.Enums;

namespace WiiTrakApi.Models
{
    [Table(name: "Carts")]
    public class CartModel : EntityModel
    {    
        [MaxLength(128)]
        public string ManufacturerName { get; set; } = string.Empty;

        public string CartNumber { get; set; } = string.Empty;

        public DateTime DateManufactured { get; set; }

        public CartOrderedFrom OrderedFrom { get; set; }

        public CartCondition Condition { get; set; }

        public CartStatus Status { get; set; }

        public string PicUrl { get; set; } = string.Empty;

        public bool IsProvisioned { get; set; }
        
        // TODO research best way to store barcode
        [MaxLength(256)]
        public string BarCode { get; set; } = string.Empty;    

        [ForeignKey(nameof(StoreModel))]
        public Guid StoreId { get; set; }

        public StoreModel? Store { get; set; }

        public TrackingDeviceModel? TrackingDevice { get; set; }

        public List<CartHistoryModel> CartHistory { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "DriverStores")]
    public class DriverStoreModel
    {
        public Guid DriverId { get; set; }

        public Guid StoreId { get; set; }

        public DriverModel? Driver { get; set; }

        public StoreModel? Store { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}

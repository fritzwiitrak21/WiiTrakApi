namespace WiiTrakApi.Models
{
    public class DriverStore
    {
        public Guid DriverId { get; set; }

        public Guid StoreId { get; set; }

        public DriverModel? Driver { get; set; }

        public StoreModel? Store { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

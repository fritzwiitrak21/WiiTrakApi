
namespace WiiTrakApi.DTOs
{
    public record DriverStoreDto
    {
        public Guid DriverId { get; set; }

        public Guid StoreId { get; set; }

        public DriverDto? Driver { get; set; }

        public StoreDto? Store { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}

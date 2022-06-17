using WiiTrakApi.Enums;
namespace WiiTrakApi.SPModels
{
    public class SPGetTrackingDeviceDetailsById
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DeviceName { get; set; }
        public string Manufactor { get; set; }
        public DateTime ManufacturedDate { get; set; }
        public DateTime InstalledDate { get; set; }
        public string SIMCardId { get; set; }
        public string SIMCardPhoneNumber { get; set; }
        public string IMEINumber { get; set; }
        public string ModelNumber { get; set; }
        public string CartNumber { get; set; }
        public string ManufacturerName { get; set; }
        public CartCondition Condition { get; set; }
        public CartOrderedFrom OrderedFrom { get; set; }
        public CartStatus Status { get; set; }
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreNumber { get; set; }
        public double StoreLatitude { get; set; }
        public double StoreLongitude { get; set; }
    }
}

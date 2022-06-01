namespace WiiTrakApi.DTOs
{
    public class DevicesDto
    {
        public Guid Id { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceName { get; set; }
        public string IMEI { get; set; }
        public string ICCID { get; set; }
        public string IMSI { get; set; }
        public Guid SIMCardId { get; set; }
        public string SIMNo { get; set; }
        public DateTime ActivatedTime { get; set; }
        public DateTime SubscriptionExpiration { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

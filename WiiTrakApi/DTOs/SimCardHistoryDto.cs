namespace WiiTrakApi.DTOs
{
    public class SimCardHistoryDto
    {
        public Guid Id { get; set; }
        public Guid SIMCardId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime? MappedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Guid TechnicianId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

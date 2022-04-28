namespace WiiTrakApi.DTOs
{
    public class CountyCodeDto
    {
        public Guid Id { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CountyName { get; set; } = string.Empty;
        public string CountyCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

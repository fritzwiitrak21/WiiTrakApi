namespace WiiTrakApi.DTOs
{
    public class SimCardsDto
    {
        public Guid Id { get; set; }
        public string TelecomCompany { get; set; }
        public string PhoneNumber { get; set; }
        public string PlanName { get; set; }
        public DateTime PlanActivationDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public string SIMNo { get; set; }
        public string IMSI { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

namespace WiiTrakApi.DTOs
{
    public record TechnicianCreationDto
    {
        public DateTime CreatedAt { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string ProfilePic { get; set; } = string.Empty;

        public Guid ServiceProviderId { get; set; }
    }
}

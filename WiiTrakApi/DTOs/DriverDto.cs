using System.ComponentModel.DataAnnotations.Schema;
using WiiTrakApi.Models;

namespace WiiTrakApi.DTOs
{
    public record DriverDto 
    {
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FirstName { get; set; } = string.Empty; 

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string ProfilePic { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.DTOs
{
    public record ProvisionDto 
    {
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

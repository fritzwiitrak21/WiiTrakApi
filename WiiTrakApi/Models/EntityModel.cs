using System.ComponentModel.DataAnnotations;

namespace WiiTrakApi.Models
{
    public abstract class EntityModel
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

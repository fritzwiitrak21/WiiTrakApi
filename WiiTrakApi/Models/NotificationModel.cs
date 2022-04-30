using System.ComponentModel.DataAnnotations.Schema;
namespace WiiTrakApi.Models
{
    [Table(name: "Notifications")]
    public class NotificationModel : EntityModel
    {
        public Guid FromId{ get; set; }
        public string Message { get; set; }= string.Empty;
        public Guid ToId { get; set; }
        public DateTime? NotifiedAt { get; set; }
        public bool IsNotified { get; set; }

    }
}

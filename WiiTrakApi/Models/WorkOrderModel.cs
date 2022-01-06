using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "WorkOrders")]
    public class WorkOrderModel : EntityModel
    {
        public int WorkOrderNumber { get; set; }

        [MaxLength(250)]
        [Required(ErrorMessage = "{0} is required.")]
        public string Issue { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Required(ErrorMessage = "{0} is required.")]
        public string Notes { get; set; } = string.Empty;

        [Url(ErrorMessage = "{0} is invalid.")]
        public string PicUrl { get; set; } = string.Empty;

        public bool IsComplete { get; set; }

        public bool IsAssigned { get; set; }

        public bool IsTrackingDeviceIssue { get; set; }

        public bool IsProvisioning { get; set; }

        public DateTime AssignedAt { get; set; }

        public DateTime CompletedAt { get; set; }

        public Guid? TechnicianId { get; set; }

        public Guid? DriverId { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? SubContractorId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? CartId { get; set; }
    }
}

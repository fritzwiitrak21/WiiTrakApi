/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace WiiTrakApi.Models
{
    [Table(name: "SimCards")]
    public class SimCardModel: EntityModel
    {
     
        public string TelecomCompany { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public DateTime PlanActivationDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public string SIMNo { get; set; } = string.Empty;
        public string IMSI { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsMapped { get; set; }
        public Guid TechnicianId { get; set; }
    }
}

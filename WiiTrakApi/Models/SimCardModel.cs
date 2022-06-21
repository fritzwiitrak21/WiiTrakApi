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
     
        public string TelecomCompany { get; set; }
        public string PhoneNumber { get; set; }
        public string PlanName { get; set; }
        public DateTime PlanActivationDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public string SIMNo { get; set; }
        public string IMSI { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}

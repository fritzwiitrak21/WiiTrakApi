/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations.Schema;
namespace WiiTrakApi.Models
{
    [Table(name: "Messages")]
    public class MessagesModel : EntityModel
    {
        public Guid SenderId { get; set; }
        public int SenderRoleId { get; set; }
        public Guid StoreId { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid ReciverId { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public bool IsActionTaken { get; set; }
        public string ActionTaken { get; set; } = string.Empty;
        public DateTime? ActionTakenAt { get; set; }
    }
}

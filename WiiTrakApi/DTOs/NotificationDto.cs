/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations.Schema;
namespace WiiTrakApi.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid FromId { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid ToId { get; set; }
        public DateTime? NotifiedAt { get; set; }
        public bool IsNotified { get; set; }
        public string MessageFrom { get; set; } = string.Empty;
        public int NotificationHour { get; set; }
    }
}

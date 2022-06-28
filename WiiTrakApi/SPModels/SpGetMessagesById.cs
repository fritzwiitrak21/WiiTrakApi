/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.SPModels
{
    public class SpGetMessagesById
    {
        public Guid Id { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid SenderId { get; set; }
        public int SenderRoleId { get; set; }
        public Guid StoreId { get; set; }
        public string Store { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Guid ReciverId { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public bool IsActionTaken { get; set; }
        public string ActionTaken { get; set; } = string.Empty;
        public DateTime? ActionTakenAt { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string ReciverName { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public string StoreNumber { get; set; } = string.Empty;

    }
}

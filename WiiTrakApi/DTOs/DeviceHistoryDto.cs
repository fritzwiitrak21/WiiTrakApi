﻿/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public class DeviceHistoryDto
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public Guid CartId { get; set; }
        public DateTime? MappedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Guid? TechnicianId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

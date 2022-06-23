/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.Models
{
    public class SimCardHistoryModel : EntityModel
    {
        public Guid SIMCardId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime MappedAt { get; set; }
        public DateTime RemovedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsActive { get; set; }

    }
}

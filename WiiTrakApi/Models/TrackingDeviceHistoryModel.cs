/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.Models
{
    public class TrackingDeviceHistoryModel
    {
        public Guid Id { get; set; }
        public Guid TrackingDeviceId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid SIMCardId { get; set; }
        public Guid CartId { get; set; }
        public DateTime? CreatedAt { get;set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

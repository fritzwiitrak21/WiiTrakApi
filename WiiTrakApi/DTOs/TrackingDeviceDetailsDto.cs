/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Enums;
namespace WiiTrakApi.DTOs
{
    public class TrackingDeviceDetailsDto
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DeviceName { get; set; }= string.Empty;
        public string Manufactor { get; set; } = string.Empty;
        public DateTime ManufacturedDate { get; set; }
        public DateTime InstalledDate { get; set; }
        public string SIMCardId { get; set; } = string.Empty;
        public string SIMCardPhoneNumber { get; set; } = string.Empty;
        public string IMEINumber { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string CartNumber { get; set; } = string.Empty;
        public string ManufacturerName { get; set; } = string.Empty;
        public CartCondition Condition { get; set; }
        public CartOrderedFrom OrderedFrom { get; set; }
        public CartStatus Status { get; set; }
        public Guid StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string StoreNumber { get; set; } = string.Empty;
        public double StoreLatitude { get; set; }
        public double StoreLongitude { get; set; }
    }
}

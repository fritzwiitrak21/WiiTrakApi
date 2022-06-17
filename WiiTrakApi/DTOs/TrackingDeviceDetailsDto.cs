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
        public string DeviceName { get; set; }=String.Empty;
        public string Manufactor { get; set; } = String.Empty;
        public DateTime ManufacturedDate { get; set; }
        public DateTime InstalledDate { get; set; }
        public string SIMCardId { get; set; } = String.Empty;
        public string SIMCardPhoneNumber { get; set; } = String.Empty;
        public string IMEINumber { get; set; } = String.Empty;
        public string ModelNumber { get; set; } = String.Empty;
        public string CartNumber { get; set; } = String.Empty;
        public string ManufacturerName { get; set; } = String.Empty;
        public CartCondition Condition { get; set; }
        public CartOrderedFrom OrderedFrom { get; set; }
        public CartStatus Status { get; set; }
        public Guid StoreId { get; set; }
        public string StoreName { get; set; } = String.Empty;
        public string StoreNumber { get; set; } = String.Empty;
        public double StoreLatitude { get; set; }
        public double StoreLongitude { get; set; }
    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Enums;
namespace WiiTrakApi.SPModels
{
    public class SpGetDeviceForStoreId
    {
        public Guid StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string StoreNumber { get; set; } = string.Empty;
        public double StoreLongitude { get; set; }
        public double StoreLatitude { get; set; }
        public Guid CartId { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public CartStatus Status { get; set; }
        public string CartNumber { get; set; } = string.Empty;
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string IMEI { get; set; } = string.Empty;
        public string FenceCoords { get; set; } = string.Empty;
        public double DeviceLongitude { get; set; }
        public double DeviceLatitude { get; set; }
    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/

namespace WiiTrakApi.DTOs
{
    public record TrackingDeviceDto
    {
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string DeviceName { get; set; } = string.Empty;

        public string Manufactor { get; set; } = string.Empty;

        public string TelecomCompanyName { get; set; } = string.Empty;

        public string SIMCardId { get; set; } = string.Empty;

        public string SIMCardPhoneNumber { get; set; } = string.Empty;

        public string IMEINumber { get; set; } = string.Empty;

        public string ModelNumber { get; set; } = string.Empty;

        public DateTime ManufacturedDate { get; set; } 

        public DateTime InstalledDate { get; set; } 
     
        public Guid CartId { get; set; }
        public bool IsActive { get; set; }
    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/

namespace WiiTrakApi.DTOs
{
    public record TrackingDeviceCreationDto
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Manufactor { get; set; } = string.Empty;

        public string TelecomCompanyName { get; set; } = string.Empty;

        public string SIMCardId { get; set; } = string.Empty;

        public string SIMCardPhoneNumber { get; set; } = string.Empty;

        public string IMEINumber { get; set; } = string.Empty;

        public string ModelNumber { get; set; } = string.Empty;

        public DateTime ManufacturedDate { get; set; } 

        public DateTime InstalledDate { get; set; } 

        public Guid CartId { get; set; }
    }
}

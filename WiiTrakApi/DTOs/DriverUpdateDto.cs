/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Enums;

namespace WiiTrakApi.DTOs
{
    public record DriverUpdateDto
    {
        public DateTime? UpdatedAt { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string StreetAddress1 { get; set; } = string.Empty;

        public string StreetAddress2 { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string CountryCode { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string ProfilePic { get; set; } = string.Empty;

        public Guid ServiceProviderId { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsActive { get; set; }
        public int DriverNumber { get; set; }
    }
}

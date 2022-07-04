/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public record StoreUpdateDto
    {

        public DateTime? UpdatedAt { get; set; }

        public string StoreName { get; set; } = string.Empty;

        public string StoreNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhonePrimary { get; set; } = string.Empty;

        public string PhoneSecondary { get; set; } = string.Empty;

        public string StreetAddress1 { get; set; } = string.Empty;

        public string StreetAddress2 { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string CountryCode { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string ProfilePicUrl { get; set; } = string.Empty;

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public Guid ServiceProviderId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CorporateId { get; set; }
        public bool IsActive { get; set; }
        public string CountyCode { get; set; } = string.Empty;
        public string ServiceFrequency { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public bool IsConnectedStore { get; set; }
    }
}

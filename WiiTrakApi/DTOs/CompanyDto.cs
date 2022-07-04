/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;

namespace WiiTrakApi.DTOs
{
    public class CompanyDto
    {
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Name { get; set; } = string.Empty;

        public string StreetAddress1 { get; set; } = string.Empty;

        public string StreetAddress2 { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string CountryCode { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string ProfilePicUrl { get; set; } = string.Empty;

        public string LogoUrl { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhonePrimary { get; set; } = string.Empty;

        public string PhoneSecondary { get; set; } = string.Empty;

        public Guid SystemOwnerId { get; set; }

        public Guid? ParentId { get; set; } = null;

        public bool IsInactive { get; set; }

        public bool CannotHaveChildren { get; set; }

        public List<ServiceProviderDto>? ServiceProviders { get; set; }

        public List<DriverDto>? Drivers { get; set; }

        public List<CorporateModel>? Corporates { get; set; }
    }
}

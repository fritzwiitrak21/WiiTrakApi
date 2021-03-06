/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public record DriverStoreDetailsDto 
    {
        public Guid Id { get; set; }//Store Id
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool DriverStoresIsActive { get; set; }
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

        public Guid DriverId { get; set; }
        public Guid AssignedBy { get; set; }
        public string AssignedDriver { get; set; } = string.Empty;
        public string CountyCode { get; set; } = string.Empty;
        public bool IsConnectedStore { get; set; }
    }
}

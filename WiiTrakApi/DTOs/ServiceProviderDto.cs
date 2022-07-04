/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;

namespace WiiTrakApi.DTOs
{
    public record ServiceProviderDto 
    {
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ServiceProviderName { get; set; } =   string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhonePrimary { get; set; } = string.Empty;

        public string PhoneSecondary { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }

        public List<StoreModel>? Stores { get; set; }
    }
}

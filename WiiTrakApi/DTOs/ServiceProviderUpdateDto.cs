/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public record ServiceProviderUpdateDto
    {
        public DateTime? UpdatedAt { get; set; }

        public string ServiceProviderName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhonePrimary { get; set; } = string.Empty;

        public string PhoneSecondary { get; set; } = string.Empty;
    }
}

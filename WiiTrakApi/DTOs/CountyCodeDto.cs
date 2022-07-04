/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public class CountyCodeDto
    {
        public Guid Id { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CountyName { get; set; } = string.Empty;
        public string CountyCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string State { get; set; }= string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

    }
}

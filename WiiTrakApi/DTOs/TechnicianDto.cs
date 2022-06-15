/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.DTOs
{
    public record TechnicianDto 
    {
        public Guid Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string ProfilePic { get; set; } = string.Empty;

        public Guid SystemOwnerId { get; set; }
        public Guid CompanyId { get; set; }
    }
}

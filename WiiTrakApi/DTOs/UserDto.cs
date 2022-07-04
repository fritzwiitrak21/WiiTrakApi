/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public record UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public int AssignedRole { get; set; }

        public DateTime PasswordLastUpdatedAt { get; set; }

        public bool IsFirstLogin { get; set; }

        public bool IsActive { get; set; }
        public string token { get; set; } = string.Empty;


    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "Users")]
    public class UsersModel : EntityModel
    {


        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public int AssignedRole { get; set; }

        public DateTime PasswordLastUpdatedAt { get; set; }

        public bool IsFirstLogin { get; set; }

        public bool IsActive { get; set; }



    }
}

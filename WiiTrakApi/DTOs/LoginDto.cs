/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordDto
    {
        [Required]
        public string Password { get; set; }
        [Required]      
        public string NewPassword { get;set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
    public class DeliveryTicketInputDto
    {
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        public int RecordCount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}

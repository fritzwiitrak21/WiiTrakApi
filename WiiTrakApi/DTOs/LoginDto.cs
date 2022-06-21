/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations;

namespace WiiTrakApi.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
    public class ChangePasswordDto
    {
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]      
        public string NewPassword { get;set; } = string.Empty;
        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
    public class DeliveryTicketInputDto
    {
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        public int RecordCount { get; set; }
        public string FromDate { get; set; } = string.Empty;
        public string ToDate { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

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
}

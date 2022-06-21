/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public class MailSettingsDto
    {
        public string Mail { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string HostUrl { get; set; } = string.Empty;
    }
    public class MailRequest
    { 
        public string MailTo { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty; //New UserName
        public bool IsForgotPasswordMail { get; set; }

    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public class MailSettingsDto
    {
        public string Mail { get; set; } 
        public string DisplayName { get; set; } 
        public string Password { get; set; } 
        public string Host { get; set; } 
        public int Port { get; set; }
        public string HostUrl { get; set; }
    }
    public class MailRequest
    { 
        public string MailTo { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } //New UserName
        public bool IsForgotPasswordMail { get; set; }

    }
}

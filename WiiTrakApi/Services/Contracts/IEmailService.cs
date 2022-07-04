/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.DTOs;

namespace WiiTrakApi.Services.Contracts
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(MailRequest Request);
        Task SendLoginCredentialsMailAsync(MailRequest Request);
    }
}

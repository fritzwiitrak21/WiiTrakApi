/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/

using WiiTrakApi.Models;
using WiiTrakApi.SPModels;
namespace WiiTrakApi.Repository.Contracts
{
    public interface IMessagesRepository
    {
        Task<(bool IsSuccess, List<MessagesModel>? Messages, string? ErrorMessage)> GetAllMessagesAsync();
        Task<(bool IsSuccess, MessagesModel? Message, string? ErrorMessage)> GetMessageAsync(Guid Id);
        Task<(bool IsSuccess, List<SpGetMessagesById>? Messages, string? ErrorMessage)> GetMessagesBIdAsync(Guid Id, int RoleId);
        Task<(bool IsSuccess, string? ErrorMessage)> AddNewMessageAsync(MessagesModel NewMessage);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateMessageAsync(MessagesModel Message);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateMessageDeliveredTimeAsync(Guid Id);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateMessageActionAsync(Guid Id, string ActionTaken);
    }
}

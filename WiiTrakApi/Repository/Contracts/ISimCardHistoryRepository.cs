/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ISimCardHistoryRepository
    {
       
        Task<(bool IsSuccess, SimCardHistoryModel? SimCardHistory, string? ErrorMessage)> GetSimCardHistoryByIdAsync(Guid id);
        Task<(bool IsSuccess, string? ErrorMessage)> CreateSimCardHistoryAsync(SimCardHistoryModel SimCardHistory);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateSimCardHistoryAsync(SimCardHistoryModel SimCardHistory);
    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IDeviceHistoryRepository
    {
        Task<(bool IsSuccess, DeviceHistoryModel? DeviceHistory, string? ErrorMessage)> GetDeviceHistoryByIdAsync(Guid id);
        Task<(bool IsSuccess, string? ErrorMessage)> CreateDeviceHistoryAsync(DeviceHistoryModel DeviceHistory);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateDeviceHistoryAsync(DeviceHistoryModel DeviceHistory);
    }
}

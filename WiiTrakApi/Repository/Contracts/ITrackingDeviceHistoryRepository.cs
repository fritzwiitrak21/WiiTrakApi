/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;
namespace WiiTrakApi.Repository.Contracts
{
    public interface ITrackingDeviceHistoryRepository
    {
        Task<(bool IsSuccess, string? ErrorMessage)> CreateTrackingDeviceHistoryAsync(TrackingDeviceHistoryModel TrackingDeviceHistory);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateTrackingDeviceHistoryAsync(TrackingDeviceHistoryModel TrackingDeviceHistory);
    }
}

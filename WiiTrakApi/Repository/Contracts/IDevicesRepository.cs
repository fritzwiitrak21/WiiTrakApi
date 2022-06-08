/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;
namespace WiiTrakApi.Repository.Contracts
{
    public interface IDevicesRepository
    {
        Task<(bool IsSuccess, List<DevicesModel>? DeviceList, string? ErrorMessage)> GetAllDeviceDetailsAsync();
        Task<(bool IsSuccess, DevicesModel? DeviceList, string? ErrorMessage)> GetDeviceByIdAsync(Guid Id);
        Task<(bool IsSuccess, string? ErrorMessage)> CreateDeviceAsync(DevicesModel Device);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateDeviceAsync(DevicesModel Device);
    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.SPModels;
namespace WiiTrakApi.Repository.Contracts
{
    public interface ITrackSolidRepository
    {
        Task<(bool IsSuccess, string? ErrorMessage)> GetDataFromTrackSolidAsync();
        Task<(bool IsSuccess, List<SpGetDeviceForStoreId>? connectedstorelist, string? ErrorMessage)> GetDeviceForStoreIdAsync();
    }
}

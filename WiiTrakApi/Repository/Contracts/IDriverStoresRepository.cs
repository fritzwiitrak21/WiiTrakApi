/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.SPModels;
using WiiTrakApi.DTOs;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IDriverStoresRepository
    {
        Task<(bool IsSuccess, List<SpGetDriverAssignedStoresByCompany>? DriverStores, string? ErrorMessage)> GetDriverStoresByCompanyIdAsync(Guid DriverId, Guid CompanyId);
        Task<(bool IsSuccess, List<SpGetDriverAssignedStoresBySystemOwner>? DriverStores, string? ErrorMessage)> GetDriverStoresBySystemOwnerIdAsync(Guid DriverId, Guid SystemOwnerId);
        Task<(bool IsSuccess, List<SpGetDriverAssignHistoryById>? DriverStoreHistory, string? ErrorMessage)> GetDriverAssignHistoryByIdAsync(Guid UserId, int RoleId);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateDriverStoresAsync(DriverStoreDetailsDto DriverStoreDetailsDto);
    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.Linq.Expressions;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IDriverRepository
    {
        Task<(bool IsSuccess, DriverModel? Driver, string? ErrorMessage)> GetDriverByIdAsync(Guid id);

        Task<(bool IsSuccess, List<DriverModel>? Drivers, string? ErrorMessage)> GetAllDriversAsync();

        Task<(bool IsSuccess, DriverReportDto? Report, string? ErrorMessage)> GetDriverReportById(Guid Id);

        Task<(bool IsSuccess, List<DriverModel>? Drivers, string? ErrorMessage)> GetDriversByConditionAsync(Expression<Func<DriverModel, bool>> expression);
        Task<(bool IsSuccess, List<DriverModel>? Drivers, string? ErrorMessage)> GetDriversBySystemOwnerAsync(Guid Id);
        Task<(bool IsSuccess, List<DriverModel>? Drivers, string? ErrorMessage)> GetDriversByStoreIdAsync(Guid StoreId);
        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> DriverExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateDriverAsync(DriverModel driver);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateDriverAsync(DriverModel driver);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteDriverAsync(Guid id);

        Task<bool> SaveAsync();
    }
}

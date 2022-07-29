/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.Linq.Expressions;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.SPModels;
namespace WiiTrakApi.Repository.Contracts
{
    public interface IStoreRepository
    {
        Task<(bool IsSuccess, StoreModel? Store, string? ErrorMessage)> GetStoreByIdAsync(Guid id);

        Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetAllStoresAsync();

        Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByConditionAsync(Expression<Func<StoreModel, bool>> expression);

        Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByCorporateId(Guid CorporateId);

        Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByCompanyId(Guid CompanyId);
        Task<(bool IsSuccess, List<SPGetStoresBySystemOwnerId>? Stores, string? ErrorMessage)> GetStoresBySystemOwnerId(Guid SystemownerId);
        Task<(bool IsSuccess, List<SpGetDriverAssignedStores>? Stores, string? ErrorMessage)> GetStoresByDriverId(Guid DriverId);

        Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetStoreReportById(Guid Id);

        Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetAllStoreReportByDriverId(Guid DriverId);

        Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetAllStoreReportByCorporateId(Guid CorporateId);

        Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetAllStoreReportByCompanyId(Guid CompanyId);
        Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByTechnicianId(Guid TechnicianId);
        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> StoreExistsAsync(Guid id);
        Task<(bool IsSuccess, List<SPGetStoreUpdateHistoryById>? StoreUpdateHistory, string? ErrorMessage)> GetStoreUpdateHistoryByIdAsync(Guid UserId, int RoleId);
        Task<(bool IsSuccess, string? ErrorMessage)> CreateStoreAsync(StoreModel store, Guid CreatedBy);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateStoreAsync(StoreModel store,Guid CreatedBy);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateStoreFenceCoordsAsync(StoreDto store);
        
        Task<(bool IsSuccess, string? ErrorMessage)> DeleteStoreAsync(Guid id);

        Task<bool> SaveAsync();
    }
}

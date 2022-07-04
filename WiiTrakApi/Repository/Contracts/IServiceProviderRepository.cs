/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IServiceProviderRepository
    {
        Task<(bool IsSuccess, ServiceProviderModel? ServiceProvider, string? ErrorMessage)> GetServiceProviderByIdAsync(Guid id);

        Task<(bool IsSuccess, List<ServiceProviderModel>? ServiceProviders, string? ErrorMessage)> GetAllServiceProvidersAsync();

        Task<(bool IsSuccess, List<ServiceProviderModel>? ServiceProviders, string? ErrorMessage)> GetServiceProvidersByConditionAsync(Expression<Func<ServiceProviderModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> ServiceProviderExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateServiceProviderAsync(ServiceProviderModel serviceProvider);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateServiceProviderAsync(ServiceProviderModel serviceProvider);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteServiceProviderAsync(Guid id);

        Task<bool> SaveAsync();
    }
}

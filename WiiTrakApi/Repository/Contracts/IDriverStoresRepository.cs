using WiiTrakApi.Models;
using WiiTrakApi.SPModels;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IDriverStoresRepository
    {
        Task<(bool IsSuccess, List<SpGetDriverAssignedStoresByCompany>? DriverStores, string? ErrorMessage)> GetDriverStoresByCompanyIdAsync(Guid DriverId, Guid CompanyId);
        Task<(bool IsSuccess, List<SpGetDriverAssignedStoresBySystemOwner>? DriverStores, string? ErrorMessage)> GetDriverStoresBySystemOwnerIdAsync(Guid DriverId, Guid SystemOwnerId);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateDriverStoresAsync(Guid DriverId, Guid StoreId, bool IsActive);
    }
}

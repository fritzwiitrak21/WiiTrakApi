using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IStoreRepository
    {
        Task<(bool IsSuccess, StoreModel? Store, string? ErrorMessage)> GetStoreByIdAsync(Guid id);

        Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetAllStoresAsync();

        Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByConditionAsync(Expression<Func<StoreModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> StoreExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateStoreAsync(StoreModel store);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateStoreAsync(StoreModel store);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteStoreAsync(Guid id);
    }
}

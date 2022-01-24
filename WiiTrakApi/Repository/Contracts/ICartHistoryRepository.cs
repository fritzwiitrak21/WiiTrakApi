using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ICartHistoryRepository
    {
        Task<(bool IsSuccess, CartHistoryModel? CartHistory, string? ErrorMessage)> GetCartHistoryByIdAsync(Guid id);

        Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetAllCartHistoryAsync();

        Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetCartHistoryByCartIdAsync(Guid cartId);

        Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetCartHistoryByStoreIdAsync(Guid storeId);

        Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetCartHistoryByServiceProviderIdAsync(Guid serviceProviderId);

        Task<(bool IsSuccess, List<CartHistoryModel>? CartHistory, string? ErrorMessage)> GetCartHistoryByConditionAsync(Expression<Func<CartHistoryModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> CartHistoryExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateCartHistoryAsync(CartHistoryModel cartHistory);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateCartHistoryAsync(CartHistoryModel cartHistory);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteCartHistoryAsync(Guid id);

        Task<bool> SaveAsync();
    }
}

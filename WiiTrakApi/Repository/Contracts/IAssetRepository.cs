using System.Linq.Expressions;
using WiiTrakApi.Models;

namespace WiiTrakApi.Repository.Contracts
{
    public interface IAssetRepository
    {
        Task<(bool IsSuccess, AssetModel? Asset, string? ErrorMessage)> GetAssetByIdAsync(Guid id);

        Task<(bool IsSuccess, List<AssetModel>? Assets, string? ErrorMessage)> GetAllAssetsAsync();

        Task<(bool IsSuccess, List<AssetModel>? Assets, string? ErrorMessage)> GetAssetsByDriverIdAsync(Guid driverId);

        Task<(bool IsSuccess, List<AssetModel>? Assets, string? ErrorMessage)> GetAssetsByConditionAsync(Expression<Func<AssetModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> AssetExistsAsync(Guid id);

        Task<(bool IsSuccess, string? ErrorMessage)> CreateAssetAsync(AssetModel asset);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateAssetAsync(AssetModel asset);

        Task<(bool IsSuccess, string? ErrorMessage)> DeleteAssetAsync(Guid id);
    }
}

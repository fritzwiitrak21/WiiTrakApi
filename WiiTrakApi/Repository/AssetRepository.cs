using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class AssetRepository: IAssetRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AssetRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, AssetModel? Asset, string? ErrorMessage)> GetAssetByIdAsync(Guid id)
        {
            var asset = await _dbContext.Assets
                .Include(x => x.TrackingDevice)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (asset is not null)
            {
                return (true, (AssetModel)asset, null);
            }
            return (false, null, "No asset found");
        }

        public async Task<(bool IsSuccess, List<AssetModel>? Assets, string? ErrorMessage)> GetAllAssetsAsync()
        {
            try
            {
                var assets = await _dbContext.Assets
                    .Include(x => x.TrackingDevice)
                    .Include(x => x.Store)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (assets.Any())
                {
                    return (true, assets, null);
                }
                return (false, null, "No assets found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<AssetModel>? Assets, string? ErrorMessage)> GetAssetsByDriverIdAsync(Guid driverId)
        {
            try
            {
                var driverStores = await _dbContext.DriverStores
                    .Where(x => x.DriverId == driverId)
                    .AsNoTracking()
                    .ToListAsync();

                var assetList = new List<AssetModel>();

                foreach (var driverStore in driverStores)
                {
                    var assets = await _dbContext.Assets
                        .Where(x => x.StoreId == driverStore.StoreId && x.Status == AssetStatus.OutsideGeofence)
                        .Include(x => x.Store)
                        .Include(x => x.TrackingDevice)
                        .AsNoTracking()
                        .ToListAsync();

                    assetList.AddRange(assets);
                }

                if (assetList.Any())
                {
                    return (true, assetList, null);
                }
                return (false, null, "No assets found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<AssetModel>? Assets, string? ErrorMessage)> GetAssetsByConditionAsync(Expression<Func<AssetModel, bool>> expression)
        {
            try
            {
                var assets = await _dbContext.Assets
                    .Include(x => x.TrackingDevice)
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (assets.Any())
                {
                    return (true, assets, null);
                }
                return (false, null, "No assets found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> AssetExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.Assets.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateAssetAsync(AssetModel asset)
        {
            try
            {
                await _dbContext.Assets.AddAsync(asset);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateAssetAsync(AssetModel asset)
        {
            try
            {
                _dbContext.Assets.Update(asset);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteAssetAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Asset not found");
                _dbContext.Assets.Remove(recordToDelete);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}

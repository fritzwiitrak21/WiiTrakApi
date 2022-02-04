using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class StoreRepository: IStoreRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StoreRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, StoreModel? Store, string? ErrorMessage)> GetStoreByIdAsync(Guid id)
        {
            var store = await _dbContext.Stores
                .Include(x => x.Carts)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (store is not null)
            {
                return (true, (StoreModel)store, null);
            }
            return (false, null, "No store found");
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetAllStoresAsync()
        {
            try
            {
                var stores = await _dbContext.Stores
                    //.Include(x => x.Carts)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (stores.Any())
                {
                    return (true, stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByDriverId(Guid driverId)
        {
            try
            {
                var driverStores = await _dbContext.DriverStores
                    .Include(x => x.Store)
                    .Where(x => x.DriverId == driverId)
                    .AsNoTracking()
                    .ToListAsync();

                var stores = driverStores.Select(x => x.Store).ToList();

                if (stores is not null && stores.Any())
                {
                    return (true, stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByConditionAsync(Expression<Func<StoreModel, bool>> expression)
        {
            try
            {
                var stores = await _dbContext.Stores
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (stores.Any())
                {
                    return (true, stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetStoreReportById(Guid Id)
        {
            try
            {
                var report = new StoreReportDto();


                //var carts = new List<CartModel>();
                var carts = new List<CartModel>();

                var storeCarts = await _dbContext.Stores
                    .Include(x => x.Carts)
                    .FirstOrDefaultAsync(x => x.Id == Id);
                carts = storeCarts.Carts;
                

                //int totalStores = storeCarts.Count();
                int totalCarts = carts.Count();
                int totalCartsAtStore = carts.Count(x => x.Status == CartStatus.InsideGeofence);
                int totalCartsOutsideStore = carts.Count(x => x.Status == CartStatus.OutsideGeofence);
                int totalCartsNeedingRepair = carts.Count(x => x.Condition == CartCondition.Damage);
                int totalCartsLost = carts.Count(x => x.Status == CartStatus.Lost);

                int cartsOnVehicleToday = 0;
                int cartsDeliveredToday = 0;
                int cartsNeedingRepairToday = 0;
                int cartsLostToday = 0;


                cartsOnVehicleToday = carts.Count(x => x.UpdatedAt is not null &&
                                                    x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                    x.Status == CartStatus.PickedUp);

                cartsDeliveredToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Status == CartStatus.InsideGeofence);

                cartsNeedingRepairToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Condition == CartCondition.Damage);

                cartsLostToday = carts.Count(x => x.UpdatedAt is not null &&
                                                  x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                  x.Status == CartStatus.Lost);

                report.TotalStores = 0;
                report.TotalCarts = totalCarts;
                report.TotalCartsAtStore = totalCartsAtStore;
                report.TotalCartsLost = totalCartsLost;
                report.TotalCartsNeedingRepair = totalCartsNeedingRepair;
                report.TotalCartsOutsideStore = totalCartsOutsideStore;

                report.CartsDeliveredToday = cartsDeliveredToday;
                report.CartsLostToday = cartsLostToday;
                report.CartsNeedingRepairToday = cartsNeedingRepairToday;
                report.CartsOnVehicleToday = cartsOnVehicleToday;

                return (true, report, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByCorporateId(Guid corporateId)
        {
            try
            {
                var corporate =
                    await _dbContext.Corporates
                        .Include(x => x.Stores)
                        .FirstOrDefaultAsync(x => x.Id == corporateId);

                if (corporate is not null && corporate.Stores.Any())
                {
                    return (true, corporate.Stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByCompanyId(Guid companyId)
        {
            try
            {
                var company =
                    await _dbContext.Companies
                        .Include(x => x.Stores)
                        .FirstOrDefaultAsync(x => x.Id == companyId);

                if (company is not null && company.Stores.Any())
                {
                    return (true, company.Stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> StoreExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.Stores.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateStoreAsync(StoreModel store)
        {
            try
            {
                await _dbContext.Stores.AddAsync(store);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateStoreAsync(StoreModel store)
        {
            try
            {
                _dbContext.Stores.Update(store);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteStoreAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.Stores.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Store not found");
                _dbContext.Stores.Remove(recordToDelete);
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

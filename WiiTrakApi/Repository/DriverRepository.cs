using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class DriverRepository: IDriverRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DriverRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, DriverModel? Driver, string? ErrorMessage)> GetDriverByIdAsync(Guid id)
        {
            var driver = await _dbContext.Drivers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (driver is not null)
            {
                return (true, (DriverModel)driver, null);
            }
            return (false, null, "No driver found");
        }

        public async Task<(bool IsSuccess, List<DriverModel>? Drivers, string? ErrorMessage)> GetAllDriversAsync()
        {
            try
            {
                var drivers = await _dbContext.Drivers
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (drivers.Any())
                {
                    return (true, drivers, null);
                }
                return (false, null, "No drivers found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<DriverModel>? Drivers, string? ErrorMessage)> GetDriversByConditionAsync(Expression<Func<DriverModel, bool>> expression)
        {
            try
            {
                var drivers = await _dbContext.Drivers
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (drivers.Any())
                {
                    return (true, drivers, null);
                }
                return (false, null, "No drivers found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> DriverExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.Drivers.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateDriverAsync(DriverModel driver)
        {
            try
            {
                await _dbContext.Drivers.AddAsync(driver);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateDriverAsync(DriverModel driver)
        {
            try
            {
                _dbContext.Drivers.Update(driver);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteDriverAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.Drivers.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Driver not found");
                _dbContext.Drivers.Remove(recordToDelete);
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

        public async Task<(bool IsSuccess, DriverReportDto? Report, string? ErrorMessage)> GetDriverReportById(Guid Id)
        {
            try
            {
                var report = new DriverReportDto();

                var corporateStores = await _dbContext.DriverStores.Where(x => x.DriverId == Id).ToListAsync();


                var carts = new List<CartModel>();

                foreach (var store in corporateStores)
                {
                    var _carts = await _dbContext.Carts
                        .Where(x => x.StoreId == store.StoreId)
                        .Include(x => x.Store)
                        .Include(x => x.TrackingDevice)
                        .AsNoTracking()
                        .ToListAsync();

                    carts.AddRange(_carts);
                }

                int totalStores = corporateStores.Count();
                int totalCarts = carts.Count();
                int totalCartsAtStore = carts.Count(x => x.Status == CartStatus.InsideGeofence);
                int totalCartsOutsideStore = carts.Count(x => x.Status == CartStatus.OutsideGeofence);
                int totalCartsNeedingRepair = carts.Count(x => x.Condition == CartCondition.Damage);
                int totalCartsGood = carts.Count(x => x.Condition == CartCondition.Good);
                int totalCartsLost = carts.Count(x => x.Status == CartStatus.Lost);
                int totalCartsTrashed = carts.Count(x => x.Status == CartStatus.Trashed);

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

                report.DriverId = Id;

                report.TotalStores = totalStores;
                report.TotalCarts = totalCarts;
                report.TotalCartsAtStore = totalCartsAtStore;
                report.TotalCartsLost = totalCartsLost;
                report.TotalCartsTrashed = totalCartsTrashed;
                report.TotalCartsGood = totalCartsGood;
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
    }
}

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
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

        public Task<(bool IsSuccess, DriverReportDto? Report, string? ErrorMessage)> GetDriverReportById(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}

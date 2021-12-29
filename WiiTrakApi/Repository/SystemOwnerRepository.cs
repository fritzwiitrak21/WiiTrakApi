using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class SystemOwnerRepository: ISystemOwnerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SystemOwnerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, SystemOwnerModel? SystemOwner, string? ErrorMessage)> GetSystemOwnerByIdAsync(Guid id)
        {
            var systemOwner = await _dbContext.SystemOwners
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (systemOwner is not null)
            {
                return (true, (SystemOwnerModel)systemOwner, null);
            }
            return (false, null, "No systemOwner found");
        }

        public async Task<(bool IsSuccess, List<SystemOwnerModel>? SystemOwners, string? ErrorMessage)> GetAllSystemOwnersAsync()
        {
            try
            {
                var systemOwners = await _dbContext.SystemOwners
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (systemOwners.Any())
                {
                    return (true, systemOwners, null);
                }
                return (false, null, "No systemOwners found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<SystemOwnerModel>? SystemOwners, string? ErrorMessage)> GetSystemOwnersByConditionAsync(Expression<Func<SystemOwnerModel, bool>> expression)
        {
            try
            {
                var systemOwners = await _dbContext.SystemOwners
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (systemOwners.Any())
                {
                    return (true, systemOwners, null);
                }
                return (false, null, "No systemOwners found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> SystemOwnerExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.SystemOwners.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateSystemOwnerAsync(SystemOwnerModel systemOwner)
        {
            try
            {
                await _dbContext.SystemOwners.AddAsync(systemOwner);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateSystemOwnerAsync(SystemOwnerModel systemOwner)
        {
            try
            {
                _dbContext.SystemOwners.Update(systemOwner);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteSystemOwnerAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.SystemOwners.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "SystemOwners not found");
                _dbContext.SystemOwners.Remove(recordToDelete);
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

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
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
                return (true, systemOwner, null);
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
                if (recordToDelete is null)
                {
                    return (false, "SystemOwners not found");
                }
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
        public async Task<(bool IsExists, string? ErrorMessage)> CheckEmailIdAsync(string EmailId)
        {
            var CheckMail = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == EmailId);
            if (CheckMail is null)
            {
                return (false, "Email not exists");
            }
            else return (true, "Email already Exists");
        }
    }
}

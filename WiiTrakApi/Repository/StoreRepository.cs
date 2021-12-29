﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WiiTrakApi.Data;
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
                    .Include(x => x.Assets)
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

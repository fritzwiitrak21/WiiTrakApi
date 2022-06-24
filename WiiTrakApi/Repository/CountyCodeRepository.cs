/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class CountyCodeRepository : ICountyCodeRepository
    {
        private readonly ApplicationDbContext DbContext;

        public CountyCodeRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<(bool IsSuccess, CountyCodeModel? CountyCode, string? ErrorMessage)> GetCountyCodeByIdAsync(Guid id)
        {
            var countycode = await DbContext.CountyCode
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (countycode is not null)
            {
                return (true, countycode, null);
            }
            return (false, null, "No countycode found");
        }
        public async Task<(bool IsSuccess, List<CountyCodeModel>? CountyList, string? ErrorMessage)> GetCountyListAsync()
        {
            var CountyCode = await DbContext.CountyCode
                                .Where(y => y.IsActive)
                                .Select(x => x)
                                .AsNoTracking().OrderBy(x=>x.City)
                                .ToListAsync();

            if (CountyCode is not null)
            {
                return (true, CountyCode, null);
            }
            return (false, null, "No countycode found");
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateCountyCodeAsync(CountyCodeModel code)
        {
            try
            {
                await DbContext.CountyCode.AddAsync(code);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateCountyCodeAsync(CountyCodeModel code)
        {
            try
            {
                DbContext.CountyCode.Update(code);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WiiTrakApi.Repository
{
   
    public class SimCardHistoryRepository : ISimCardHistoryRepository
    {
        private readonly ApplicationDbContext DbContext;
        public SimCardHistoryRepository(ApplicationDbContext dbcontext)
        {
            DbContext = dbcontext;
        }
        
        public async Task<(bool IsSuccess, SimCardHistoryModel? SimCardHistory, string? ErrorMessage)> GetSimCardHistoryByIdAsync(Guid id)
        {
            var SimCardHistory = await DbContext.SimCardHistory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (SimCardHistory is not null)
            {
                return (true, SimCardHistory, null);
            }
            return (false, null, "No Simcard history found");
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateSimCardHistoryAsync(SimCardHistoryModel SimCardHistory)
        {
            try
            {
                await DbContext.SimCardHistory.AddAsync(SimCardHistory);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateSimCardHistoryAsync(SimCardHistoryModel SimCardHistory)
        {
            try
            {
                DbContext.SimCardHistory.Update(SimCardHistory);
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

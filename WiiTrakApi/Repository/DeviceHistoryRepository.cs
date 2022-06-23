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
   
    public class DeviceHistoryRepository : IDeviceHistoryRepository
    {
        private readonly ApplicationDbContext DbContext;
        public DeviceHistoryRepository(ApplicationDbContext dbcontext)
        {
            DbContext = dbcontext;
        }
       
        public async Task<(bool IsSuccess, DeviceHistoryModel? DeviceHistory, string? ErrorMessage)> GetDeviceHistoryByIdAsync(Guid id)
        {
            var DeviceHistory = await DbContext.DeviceHistory
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (DeviceHistory is not null)
            {
                return (true, DeviceHistory, null);
            }
            return (false, null, "No Device history found");
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateDeviceHistoryAsync(DeviceHistoryModel DeviceHistory)
        {
            try
            {
                await DbContext.DeviceHistory.AddAsync(DeviceHistory);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateDeviceHistoryAsync(DeviceHistoryModel DeviceHistory)
        {
            try
            {
                DbContext.DeviceHistory.Update(DeviceHistory);
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

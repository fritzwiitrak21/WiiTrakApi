/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class TrackingDeviceHistoryRepository: ITrackingDeviceHistoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TrackingDeviceHistoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateTrackingDeviceHistoryAsync(TrackingDeviceHistoryModel TrackingDeviceHistory)
        {
            try
            {
                await _dbContext.TrackingDeviceHistory.AddAsync(TrackingDeviceHistory);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateTrackingDeviceHistoryAsync(TrackingDeviceHistoryModel TrackingDeviceHistory)
        {
            try
            {
                _dbContext.TrackingDeviceHistory.Update(TrackingDeviceHistory);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

    }
}

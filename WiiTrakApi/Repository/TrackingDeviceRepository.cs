﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class TrackingDeviceRepository: ITrackingDeviceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TrackingDeviceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, TrackingDeviceModel? TrackingDevice, string? ErrorMessage)> GetTrackingDeviceByIdAsync(Guid id)
        {
            var trackingDevice = await _dbContext.TrackingDevices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (trackingDevice is not null)
            {
                return (true, (TrackingDeviceModel)trackingDevice, null);
            }
            return (false, null, "No tracking device found");
        }

        public async Task<(bool IsSuccess, List<TrackingDeviceModel>? TrackingDevices, string? ErrorMessage)> GetAllTrackingDevicesAsync()
        {
            try
            {
                var trackingDevices = await _dbContext.TrackingDevices
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (trackingDevices.Any())
                {
                    return (true, trackingDevices, null);
                }
                return (false, null, "No tracking devices found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<TrackingDeviceModel>? TrackingDevices, string? ErrorMessage)> GetTrackingDevicesByConditionAsync(Expression<Func<TrackingDeviceModel, bool>> expression)
        {
            try
            {
                var trackingDevices = await _dbContext.TrackingDevices
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (trackingDevices.Any())
                {
                    return (true, trackingDevices, null);
                }
                return (false, null, "No tracking devices found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> TrackingDeviceExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.TrackingDevices.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateTrackingDeviceAsync(TrackingDeviceModel trackingDevice)
        {
            try
            {
                await _dbContext.TrackingDevices.AddAsync(trackingDevice);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateTrackingDeviceAsync(TrackingDeviceModel trackingDevice)
        {
            try
            {
                _dbContext.TrackingDevices.Update(trackingDevice);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteTrackingDeviceAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.TrackingDevices.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Tracking Device not found");
                _dbContext.TrackingDevices.Remove(recordToDelete);
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

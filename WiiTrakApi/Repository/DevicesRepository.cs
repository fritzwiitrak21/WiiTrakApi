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
  

    public class DevicesRepository: IDevicesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DevicesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(bool IsSuccess, List<DevicesModel>? DeviceList, string? ErrorMessage)> GetAllDeviceDetailsAsync()
        {
            var DeviceList = await _dbContext.Devices
                                .Where(y => y.IsActive)
                                .Select(x => x)
                                .AsNoTracking()
                                .ToListAsync();
            if (DeviceList is not null)
            {
                return (true, DeviceList, null);
            }
            return (false, null, "No Devices found");
        }
        public async Task<(bool IsSuccess, DevicesModel? DeviceList, string? ErrorMessage)> GetDeviceByIdAsync(Guid Id)
        {
            var DeviceList = await _dbContext.Devices
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.IsActive && x.Id == Id);
            if (DeviceList is not null)
            {
                return (true, DeviceList, null);
            }
            return (false, null, "No Device found");
        }
        public async Task<(bool IsSuccess, List<DevicesModel>? Devices, string? ErrorMessage)> GetDeviceByTechnicianIdAsync(Guid TechnicianId)
        {
            try
            {
                var devices = await _dbContext.Devices
                    .Where(x => x.TechnicianId == TechnicianId)
                    .AsNoTracking().OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
                if (devices.Any())
                {
                    return (true, devices, null);
                }
                return (false, null, "No Device found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateDeviceAsync(DevicesModel Device)
        {
            try
            {
                await _dbContext.Devices.AddAsync(Device);
                #region Add Simcard History
                SimCardHistoryModel SimCardHistory = new SimCardHistoryModel();
                SimCardHistory.DeviceId=Device.Id;
                SimCardHistory.SIMCardId = Device.SIMCardId;
                SimCardHistory.MappedAt = DateTime.UtcNow;
                SimCardHistory.CreatedAt = DateTime.UtcNow;
                SimCardHistory.IsActive = true;
                SimCardHistory.TechnicianId = Device.TechnicianId;
                await _dbContext.SimCardHistory.AddAsync(SimCardHistory);
                #endregion
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateDeviceAsync(DevicesModel Device)
        {
            try
            {
                _dbContext.Devices.Update(Device);
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

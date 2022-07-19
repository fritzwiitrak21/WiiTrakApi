/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.SPModels;
using Microsoft.Data.SqlClient;

namespace WiiTrakApi.Repository
{
    public class TrackingDeviceRepository : ITrackingDeviceRepository
    {
        private readonly ApplicationDbContext DbContext;

        public TrackingDeviceRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<(bool IsSuccess, TrackingDeviceModel? TrackingDevice, string? ErrorMessage)> GetTrackingDeviceByIdAsync(Guid id)
        {
            var trackingDevice = await DbContext.TrackingDevices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (trackingDevice is not null)
            {
                return (true, trackingDevice, null);
            }
            return (false, null, "No tracking device found");
        }
        public async Task<(bool IsSuccess, TrackingDeviceModel? TrackingDevice, string? ErrorMessage)> GetTrackingDevicebyIMEIAsync(string IMEI)
        {
            var trackingDevice = await DbContext.TrackingDevices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IMEINumber == IMEI);

            if (trackingDevice is not null)
            {
                return (true, trackingDevice, null);
            }
            return (false, null, "No tracking device found");
        }
        
        public async Task<(bool IsSuccess, List<TrackingDeviceModel>? TrackingDevices, string? ErrorMessage)> GetAllTrackingDevicesAsync()
        {
            try
            {
                var trackingDevices = await DbContext.TrackingDevices
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
                var trackingDevices = await DbContext.TrackingDevices
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
                var exists = await DbContext.TrackingDevices.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<SPGetTrackingDeviceDetailsById>? TrackingDeviceDetails, string? ErrorMessage)> GetTrackingDeviceDetailsByIdAsync(Guid Id, Enums.Role role)
        {
            try
            {
                List<SqlParameter> parms;
                const string sqlquery = "Exec SPGetTrackingDeviceDetailsById @Id,@RoleId";
                parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Id", Value =Id  },
                    new SqlParameter { ParameterName = "@RoleId", Value =(int)role },

                };

                var TrackingDeviceDetails = await DbContext.SPGetTrackingDeviceDetailsById.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

                if (TrackingDeviceDetails != null)
                {
                    return (true, TrackingDeviceDetails, null);
                }
                return (false, null, "No Tracking Device found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<SPGetTrackingDeviceDetailsById>? TrackingDeviceDetails, string? ErrorMessage)> GetTrackingDeviceDetailsByIdDriverAsync(Guid Id)
        {
            try
            {
                List<SqlParameter> parms;
                const string sqlquery = "Exec SPGetTrackingDeviceDetailsByDriverId @DriverId";
                parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@DriverId", Value =Id  },
                };

                var TrackingDeviceDetails = await DbContext.SPGetTrackingDeviceDetailsById.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

                if (TrackingDeviceDetails != null)
                {
                    return (true, TrackingDeviceDetails, null);
                }
                return (false, null, "No Tracking Device found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateTrackingDeviceAsync(TrackingDeviceModel TrackingDevice)
        {
            try
            {
                await DbContext.TrackingDevices.AddAsync(TrackingDevice);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateTrackingDeviceAsync(TrackingDeviceModel TrackingDevice)
        {
            try
            {
                DbContext.TrackingDevices.Update(TrackingDevice);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateTrackingDeviceCoOrdinatesAsync(TrackingDeviceModel TrackingDevice)
        {
            try
            {
                const string sqlquery = "Exec SPUpdateTrackingDeviceByIMEI @IMEINumber,@DeviceName,@Latitude,@Longitude";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@IMEINumber", Value = TrackingDevice.IMEINumber},
                     new SqlParameter { ParameterName = "@DeviceName", Value = TrackingDevice.DeviceName },
                     new SqlParameter { ParameterName = "@Latitude", Value = TrackingDevice.Latitude },
                     new SqlParameter { ParameterName = "@Longitude", Value = TrackingDevice.Longitude }
                };
                var Result = await DbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());
                await DbContext.SaveChangesAsync();
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
                var recordToDelete = await DbContext.TrackingDevices.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null)
                {
                    return (false, "Tracking Device not found");
                }
                DbContext.TrackingDevices.Remove(recordToDelete);
                await DbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await DbContext.SaveChangesAsync() >= 0;
        }
    }
}

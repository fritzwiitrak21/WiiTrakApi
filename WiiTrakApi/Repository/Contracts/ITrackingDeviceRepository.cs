/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.Linq.Expressions;
using WiiTrakApi.Models;
using WiiTrakApi.SPModels;

namespace WiiTrakApi.Repository.Contracts
{
    public interface ITrackingDeviceRepository
    {
        Task<(bool IsSuccess, TrackingDeviceModel? TrackingDevice, string? ErrorMessage)> GetTrackingDeviceByIdAsync(Guid id);
        Task<(bool IsSuccess, TrackingDeviceModel? TrackingDevice, string? ErrorMessage)> GetTrackingDevicebyIMEIAsync(string IMEI);
        Task<(bool IsSuccess, List<TrackingDeviceModel>? TrackingDevices, string? ErrorMessage)> GetAllTrackingDevicesAsync();

        Task<(bool IsSuccess, List<TrackingDeviceModel>? TrackingDevices, string? ErrorMessage)> GetTrackingDevicesByConditionAsync(Expression<Func<TrackingDeviceModel, bool>> expression);

        Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> TrackingDeviceExistsAsync(Guid id);
        Task<(bool IsSuccess, List<SPGetTrackingDeviceDetailsById>? TrackingDeviceDetails, string? ErrorMessage)> GetTrackingDeviceDetailsByIdAsync(Guid Id, Enums.Role role);
        Task<(bool IsSuccess, List<SPGetTrackingDeviceDetailsById>? TrackingDeviceDetails, string? ErrorMessage)> GetTrackingDeviceDetailsByIdDriverAsync(Guid Id);
        Task<(bool IsSuccess, string? ErrorMessage)> CreateTrackingDeviceAsync(TrackingDeviceModel trackingDevice);

        Task<(bool IsSuccess, string? ErrorMessage)> UpdateTrackingDeviceAsync(TrackingDeviceModel trackingDevice);
        Task<(bool IsSuccess, string? ErrorMessage)> UpdateTrackingDeviceCoOrdinatesAsync(TrackingDeviceModel trackingDevice);
        Task<(bool IsSuccess, string? ErrorMessage)> DeleteTrackingDeviceAsync(Guid id);

        Task<bool> SaveAsync();
    }
}

/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/devices")]
    [ApiController]

    public class DevicesController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly IDevicesRepository Repository;
        private readonly ISimCardsRepository SimCardsRepository;
        private readonly ISimCardHistoryRepository SimCardHistoryRepository;
        private readonly ITrackingDeviceRepository TrackingDeviceRepository;
        private readonly ITrackingDeviceHistoryRepository TrackingDeviceHistoryRepository;
        private readonly ITechnicianRepository TechnicianRepository;
        public DevicesController(IMapper mapper, IDevicesRepository repository, ISimCardsRepository simrepository, ISimCardHistoryRepository simcardhistory, ITrackingDeviceRepository Trackingdevicerepository, ITrackingDeviceHistoryRepository Trackingdevicehistoryrepository, ITechnicianRepository Technicianrepository)
        {
            Mapper = mapper;
            Repository = repository;
            SimCardsRepository = simrepository;
            SimCardHistoryRepository = simcardhistory;
            TrackingDeviceRepository = Trackingdevicerepository;
            TrackingDeviceHistoryRepository = Trackingdevicehistoryrepository;
            TechnicianRepository = Technicianrepository;
        }
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllDeviceDetails()
        {
            var result = await Repository.GetAllDeviceDetailsAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtolist = Mapper.Map<List<DevicesDto>>(result.DeviceList);
            return Ok(dtolist);
        }

        [HttpGet("{id:guid}", Name = "GetDevice")]
        public async Task<IActionResult> GetDeviceById(Guid id)
        {
            var result = await Repository.GetDeviceByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<DevicesDto>(result.DeviceList);
            return Ok(dto);
        }
        [HttpGet("Technicians/{TechnicianId:guid}")]
        public async Task<IActionResult> GetDeviceByTechnicianId(Guid TechnicianId)
        {
            var result = await Repository.GetDeviceByTechnicianIdAsync(TechnicianId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DevicesDto>>(result.Devices);

            return Ok(dtoList);
        }
        [HttpPost]
        public async Task<ActionResult<DevicesDto>> CreateDevice([FromBody] DevicesDto DeviceCreation)
        {
            var Device = Mapper.Map<DevicesModel>(DeviceCreation);
            Device.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateDeviceAsync(Device);
            var CurrentSim = await SimCardsRepository.GetSimCardByIdAsync(DeviceCreation.SIMCardId);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            CurrentSim.SimCardList.IsMapped = true;
            CurrentSim.SimCardList.UpdatedAt = DateTime.UtcNow;
            await SimCardsRepository.UpdateSimCardAsync(CurrentSim.SimCardList);
            var TechnicianDetails = await TechnicianRepository.GetTechnicianByIdAsync(DeviceCreation.TechnicianId);
            var TrackingDevice = new TrackingDeviceModel
            {
                DeviceName = DeviceCreation.DeviceName,
                Manufactor = "Jimi IOT",
                TelecomCompanyName = CurrentSim.SimCardList.TelecomCompany,
                SIMCardId = (CurrentSim.SimCardList.Id).ToString(),
                SIMCardPhoneNumber = CurrentSim.SimCardList.PhoneNumber,
                IMEINumber = DeviceCreation.IMEI,
                ModelNumber = DeviceCreation.DeviceModel,
                CartId = Guid.Empty,
                Latitude = 0,
                Longitude = 0,
                CreatedAt = DateTime.UtcNow,
                SystemOwnerId = new Guid(TechnicianDetails.Technician.SystemOwnerId.ToString()),
                IsActive=true
            };
            await TrackingDeviceRepository.CreateTrackingDeviceAsync(TrackingDevice);
            var TrackingDeviceHistory = new TrackingDeviceHistoryModel
            {
                TrackingDeviceId = TrackingDevice.Id,
                Longitude = TrackingDevice.Longitude,
                Latitude = TrackingDevice.Latitude,
                SIMCardId = new Guid(TrackingDevice.SIMCardId),
                CartId = TrackingDevice.CartId,
                CreatedAt = DateTime.UtcNow
            };
            await TrackingDeviceHistoryRepository.CreateTrackingDeviceHistoryAsync(TrackingDeviceHistory);
            var dto = Mapper.Map<DevicesDto>(Device);
            return CreatedAtRoute(nameof(GetDeviceById), new { id = dto.Id }, dto);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDevice(Guid id, DevicesDto DeviceUpdate)
        {
            var result = await Repository.GetDeviceByIdAsync(id);
            var CurrentSim = await SimCardsRepository.GetSimCardByIdAsync(DeviceUpdate.SIMCardId);
            var PreviousSim = await SimCardsRepository.GetSimCardByIdAsync(result.DeviceList.SIMCardId);
            var PreviousSimHistory = new SimCardHistoryModel();
            var CurrentSimHistory = new SimCardHistoryModel();
            if (!result.IsSuccess || result.DeviceList is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(DeviceUpdate, result.DeviceList);
            result.DeviceList.UpdatedAt = DateTime.UtcNow;
            var updateResult = await Repository.UpdateDeviceAsync(result.DeviceList);
            #region Update Tracking Device
            var TrackingDevice = await TrackingDeviceRepository.GetTrackingDevicebyIMEIAsync(DeviceUpdate.IMEI);
            if (TrackingDevice.TrackingDevice != null)
            {
                TrackingDevice.TrackingDevice.SIMCardId = DeviceUpdate.SIMCardId.ToString();
                TrackingDevice.TrackingDevice.UpdatedAt = DateTime.UtcNow;
                if (CurrentSim.SimCardList != null)
                {
                    TrackingDevice.TrackingDevice.SIMCardPhoneNumber = CurrentSim.SimCardList.PhoneNumber;
                    TrackingDevice.TrackingDevice.TelecomCompanyName = CurrentSim.SimCardList.TelecomCompany;
                }
                else
                {
                    TrackingDevice.TrackingDevice.SIMCardPhoneNumber = "";
                    TrackingDevice.TrackingDevice.TelecomCompanyName = "";
                }
                await TrackingDeviceRepository.UpdateTrackingDeviceAsync(TrackingDevice.TrackingDevice);
                var TrackingDeviceHistory = new TrackingDeviceHistoryModel
                {
                    TrackingDeviceId = TrackingDevice.TrackingDevice.Id,
                    Longitude = TrackingDevice.TrackingDevice.Longitude,
                    Latitude = TrackingDevice.TrackingDevice.Latitude,
                    SIMCardId = new Guid(TrackingDevice.TrackingDevice.SIMCardId),
                    CartId = TrackingDevice.TrackingDevice.CartId,
                    CreatedAt = DateTime.UtcNow
                };
                await TrackingDeviceHistoryRepository.CreateTrackingDeviceHistoryAsync(TrackingDeviceHistory);
            }
            
            #endregion
            if (updateResult.IsSuccess)
            {

                if (PreviousSim.SimCardList != null)
                {
                    PreviousSimHistory.DeviceId = DeviceUpdate.Id;
                    PreviousSimHistory.SIMCardId = PreviousSim.SimCardList.Id;
                    PreviousSimHistory.RemovedAt = DateTime.UtcNow;
                    PreviousSimHistory.CreatedAt = DateTime.UtcNow;
                    PreviousSimHistory.IsActive = true;
                    PreviousSimHistory.TechnicianId = DeviceUpdate.TechnicianId;
                }
                if (CurrentSim.SimCardList != null)
                {
                    CurrentSimHistory.DeviceId = DeviceUpdate.Id;
                    CurrentSimHistory.SIMCardId = DeviceUpdate.SIMCardId;
                    CurrentSimHistory.MappedAt = DateTime.UtcNow;
                    CurrentSimHistory.CreatedAt = DateTime.UtcNow;
                    CurrentSimHistory.IsActive = true;
                    CurrentSimHistory.TechnicianId = DeviceUpdate.TechnicianId;
                }
                if (DeviceUpdate.SIMCardId != Guid.Empty)
                {

                    if (PreviousSim.SimCardList != null && CurrentSim.SimCardList != null && CurrentSim.SimCardList.Id != PreviousSim.SimCardList.Id)
                    {
                        PreviousSim.SimCardList.IsMapped = false;
                        PreviousSim.SimCardList.UpdatedAt = DateTime.UtcNow;
                        CurrentSim.SimCardList.IsMapped = true;
                        CurrentSim.SimCardList.UpdatedAt = DateTime.UtcNow;
                        await SimCardHistoryRepository.CreateSimCardHistoryAsync(PreviousSimHistory);
                        await SimCardHistoryRepository.CreateSimCardHistoryAsync(CurrentSimHistory);
                        await SimCardsRepository.UpdateSimCardAsync(PreviousSim.SimCardList);
                        await SimCardsRepository.UpdateSimCardAsync(CurrentSim.SimCardList);
                    }
                    else if (PreviousSim.SimCardList != null)
                    {
                        PreviousSim.SimCardList.IsMapped = false;
                        PreviousSim.SimCardList.UpdatedAt = DateTime.UtcNow;
                        await SimCardHistoryRepository.CreateSimCardHistoryAsync(PreviousSimHistory);
                        await SimCardsRepository.UpdateSimCardAsync(PreviousSim.SimCardList);
                    }
                    else if (CurrentSim.SimCardList != null)
                    {
                        CurrentSim.SimCardList.IsMapped = true;
                        CurrentSim.SimCardList.UpdatedAt = DateTime.UtcNow;
                        await SimCardHistoryRepository.CreateSimCardHistoryAsync(CurrentSimHistory);
                        await SimCardsRepository.UpdateSimCardAsync(CurrentSim.SimCardList);
                    }
                }
                else
                {
                    PreviousSim.SimCardList.IsMapped = false;
                    PreviousSim.SimCardList.UpdatedAt = DateTime.UtcNow;
                    await SimCardHistoryRepository.CreateSimCardHistoryAsync(PreviousSimHistory);
                    await SimCardsRepository.UpdateSimCardAsync(PreviousSim.SimCardList);
                }

                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

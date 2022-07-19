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
using WiiTrakApi.Enums; 

namespace WiiTrakApi.Controllers
{
    [Route("api/trackingdevice")]
    [ApiController]
    public class TrackingDeviceController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ITrackingDeviceRepository Repository;
        private readonly ITrackingDeviceHistoryRepository HistoryRepository;
       

        public TrackingDeviceController(IMapper mapper, ITrackingDeviceRepository repository, ITrackingDeviceHistoryRepository historyrepository)
        {
            Mapper = mapper;
            Repository = repository;
            HistoryRepository = historyrepository;
        }

        [HttpGet("{id:guid}", Name = "GetTrackingDevice")]
        public async Task<IActionResult> GetTrackingDevice(Guid id)
        {
            var result = await Repository.GetTrackingDeviceByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<TrackingDeviceDto>(result.TrackingDevice);
            return Ok(dto);
        }
        [HttpGet("IMEI")]
        public async Task<IActionResult> GetTrackingDevicebyIMEI(string IMEI)
        {
            var result = await Repository.GetTrackingDevicebyIMEIAsync(IMEI);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<TrackingDeviceDto>(result.TrackingDevice);
            return Ok(dto);
        }


        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllTrackingDevices()
        {
            var result = await Repository.GetAllTrackingDevicesAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<TrackingDeviceDto>>(result.TrackingDevices);
            return Ok(dtoList);
        }
        
        [HttpGet("TrackingDevice/{id:guid}/{RoleId:int}")]
        public async Task<IActionResult> GetTrackingDeviceDetailsById(Guid id, int RoleId)
        {
            var result = await Repository.GetTrackingDeviceDetailsByIdAsync(id, (Role)RoleId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<TrackingDeviceDetailsDto>>(result.TrackingDeviceDetails);
            return Ok(dtoList);
        }
        [HttpGet("CartDetails/{id:guid}")]
        public async Task<IActionResult> GetTrackingDeviceDetailsByDriverId(Guid id)
        {
            var result = await Repository.GetTrackingDeviceDetailsByIdDriverAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<TrackingDeviceDetailsDto>>(result.TrackingDeviceDetails);
            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackingDeviceDto>> CreateTrackingDevice([FromBody] TrackingDeviceDto TrackingDeviceCreation)
        {
            var TrackingDevice = Mapper.Map<TrackingDeviceModel>(TrackingDeviceCreation);
            TrackingDevice.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateTrackingDeviceAsync(TrackingDevice);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            var dto = Mapper.Map<TrackingDeviceDto>(TrackingDevice);
            var TrackingDeviceHistory = new TrackingDeviceHistoryModel
            {
                TrackingDeviceId=dto.Id,
                Longitude=dto.Longitude,
                Latitude=dto.Latitude,
                SIMCardId=new Guid(dto.SIMCardId),
                CartId=dto.CartId,
                CreatedAt=DateTime.UtcNow
            };
            await HistoryRepository.CreateTrackingDeviceHistoryAsync(TrackingDeviceHistory);
            return CreatedAtRoute(nameof(GetTrackingDevice), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTrackingDevice(Guid id, TrackingDeviceDto TrackingDeviceUpdate)
        {
            var result = await Repository.GetTrackingDeviceByIdAsync(id);
            if (!result.IsSuccess || result.TrackingDevice is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(TrackingDeviceUpdate, result.TrackingDevice);
            result.TrackingDevice.UpdatedAt = DateTime.UtcNow;
            var updateResult = await Repository.UpdateTrackingDeviceAsync(result.TrackingDevice);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCoordinatesOfDevices(TrackingDeviceDto TrackingDeviceUpdate)
        {
            var TrackingDevice = new TrackingDeviceModel();
            Mapper.Map(TrackingDeviceUpdate, TrackingDevice);
            var updateResult = await Repository.UpdateTrackingDeviceCoOrdinatesAsync(TrackingDevice);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrackingDevice(Guid id)
        {
            var result = await Repository.DeleteTrackingDeviceAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

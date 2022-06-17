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

        public TrackingDeviceController(IMapper mapper,
            ITrackingDeviceRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
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
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackingDeviceDto>> CreateTrackingDevice([FromBody] TrackingDeviceCreationDto trackingDeviceCreation)
        {
            var trackingDevice = Mapper.Map<TrackingDeviceModel>(trackingDeviceCreation);
            trackingDevice.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateTrackingDeviceAsync(trackingDevice);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            var dto = Mapper.Map<TrackingDeviceDto>(trackingDevice);
            return CreatedAtRoute(nameof(GetTrackingDevice), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTrackingDevice(Guid id, TrackingDeviceUpdateDto trackingDeviceUpdate)
        {
            var result = await Repository.GetTrackingDeviceByIdAsync(id);
            if (!result.IsSuccess || result.TrackingDevice is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(trackingDeviceUpdate, result.TrackingDevice);
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
        public async Task<IActionResult> GetCoordinatesOfDevices()
        {
            //var result = await _repository.GetTrackingDeviceByIdAsync(id);
            //if (!result.IsSuccess || result.TrackingDevice is null) return NotFound(result.ErrorMessage);
            //_mapper.Map(trackingDeviceUpdate, result.TrackingDevice);
            //result.TrackingDevice.UpdatedAt = DateTime.UtcNow;
            //var updateResult = await _repository.UpdateTrackingDeviceAsync(result.TrackingDevice);
            //if (updateResult.IsSuccess) return NoContent();
            //error
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

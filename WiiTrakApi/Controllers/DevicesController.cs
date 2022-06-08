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
        private readonly IMapper _mapper;
        private readonly IDevicesRepository _repository;
        public DevicesController(IMapper mapper, IDevicesRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllDeviceDetails()
        {
            var result = await _repository.GetAllDeviceDetailsAsync();
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtolist = _mapper.Map<List<DevicesDto>>(result.DeviceList);
            return Ok(dtolist);
        }

        [HttpGet("{id:guid}", Name = "GetDevice")]
        public async Task<IActionResult> GetDeviceById(Guid id)
        {
            var result = await _repository.GetDeviceByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<DevicesDto>(result.DeviceList);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<DevicesDto>> CreateDevice([FromBody] DeviceCreationDto DeviceCreation)
        {
            var Device = _mapper.Map<DevicesModel>(DeviceCreation);
            Device.CreatedAt = DateTime.UtcNow;
            var createResult = await _repository.CreateDeviceAsync(Device);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<DevicesDto>(Device);
            return CreatedAtRoute(nameof(GetDeviceById), new { id = dto.Id }, dto);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDevice(Guid id, DeviceUpdateDto DeviceUpdate)
        {
            var result = await _repository.GetDeviceByIdAsync(id);

            if (!result.IsSuccess || result.DeviceList is null) return NotFound(result.ErrorMessage);
            _mapper.Map(DeviceUpdate, result.DeviceList);
            result.DeviceList.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateDeviceAsync(result.DeviceList);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

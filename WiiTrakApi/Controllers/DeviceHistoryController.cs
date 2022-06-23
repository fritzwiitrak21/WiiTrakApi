/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using AutoMapper;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/devicehistory")]
    [ApiController]
    public class DeviceHistoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDeviceHistoryRepository _repository;

        public DeviceHistoryController(IMapper mapper, IDeviceHistoryRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
       
        [EnableQuery]
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetDeviceHistoryById(Guid Id)
        {
            var result = await _repository.GetDeviceHistoryByIdAsync(Id);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            var dtoList = _mapper.Map<List<DeviceHistoryDto>>(result.DeviceHistory);

            return Ok(dtoList);
        }
        [HttpPost]
        public async Task<ActionResult<DeviceHistoryDto>> CreateDeviceHistory([FromBody] DeviceHistoryDto DeviceHistoryCreation)
        {
            var DeviceHistory = _mapper.Map<DeviceHistoryModel>(DeviceHistoryCreation);
            DeviceHistory.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateDeviceHistoryAsync(DeviceHistory);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<DeviceHistoryDto>(DeviceHistory);
            return CreatedAtRoute(nameof(GetDeviceHistoryById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDeviceHistory(Guid id, DeviceHistoryDto DeviceHistoryUpdate)
        {
            var result = await _repository.GetDeviceHistoryByIdAsync(id);

            if (!result.IsSuccess || result.DeviceHistory is null)
            {
                return NotFound(result.ErrorMessage);
            }
            _mapper.Map(DeviceHistoryUpdate, result.DeviceHistory);
            result.DeviceHistory.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateDeviceHistoryAsync(result.DeviceHistory);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

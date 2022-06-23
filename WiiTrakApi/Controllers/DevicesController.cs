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
        public DevicesController(IMapper mapper, IDevicesRepository repository, ISimCardsRepository simrepository)
        {
            Mapper = mapper;
            Repository = repository;
            SimCardsRepository = simrepository;
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
            var dto = Mapper.Map<DevicesDto>(Device);
            return CreatedAtRoute(nameof(GetDeviceById), new { id = dto.Id }, dto);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDevice(Guid id, DevicesDto DeviceUpdate)
        {
            var result = await Repository.GetDeviceByIdAsync(id);
            var CurrentSim = await SimCardsRepository.GetSimCardByIdAsync(DeviceUpdate.SIMCardId);
            var PreviousSim = await SimCardsRepository.GetSimCardByIdAsync(result.DeviceList.SIMCardId);
            if (!result.IsSuccess || result.DeviceList is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(DeviceUpdate, result.DeviceList);
            result.DeviceList.UpdatedAt = DateTime.UtcNow;
            var updateResult = await Repository.UpdateDeviceAsync(result.DeviceList);
            if (updateResult.IsSuccess)
            {
                if (CurrentSim.SimCardList.Id != PreviousSim.SimCardList.Id)
                {
                    PreviousSim.SimCardList.IsMapped = false;
                    PreviousSim.SimCardList.UpdatedAt = DateTime.UtcNow;
                    await SimCardsRepository.UpdateSimCardAsync(PreviousSim.SimCardList);
                }
                CurrentSim.SimCardList.IsMapped = true;
                CurrentSim.SimCardList.UpdatedAt = DateTime.UtcNow;
                await SimCardsRepository.UpdateSimCardAsync(CurrentSim.SimCardList);
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

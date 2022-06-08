/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/driverstores")]
    [ApiController]
    public class DriverStoresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDriverStoresRepository _repository;

        public DriverStoresController(IMapper mapper, IDriverStoresRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("Company/{CompanyId:guid}/{DriverId:guid}")]
        public async Task<ActionResult> GetDriverStoresByCompanyId(Guid CompanyId, Guid DriverId)
        {
            var result = await _repository.GetDriverStoresByCompanyIdAsync(DriverId, CompanyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverStoreDetailsDto>>(result.DriverStores);

            return Ok(dtoList);
        }

        [HttpGet("SystemOwner/{SystemOwnerId:guid}/{DriverId:guid}")]
        public async Task<ActionResult> GetDriverStoresBySystemOwnerId(Guid SystemOwnerId, Guid DriverId)
        {
            var result = await _repository.GetDriverStoresBySystemOwnerIdAsync(DriverId, SystemOwnerId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverStoreDetailsDto>>(result.DriverStores);

            return Ok(dtoList);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDeliveryTicket(DriverStoreDetailsDto DriverStoreDetailsDto)
        {
            var result = await _repository.UpdateDriverStoresAsync(DriverStoreDetailsDto);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

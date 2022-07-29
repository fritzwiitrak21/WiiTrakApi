/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.DTOs;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/driverstores")]
    [ApiController]
    public class DriverStoresController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly IDriverStoresRepository Repository;

        public DriverStoresController(IMapper mapper, IDriverStoresRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet("Company/{CompanyId:guid}/{DriverId:guid}")]
        public async Task<ActionResult> GetDriverStoresByCompanyId(Guid CompanyId, Guid DriverId)
        {
            var result = await Repository.GetDriverStoresByCompanyIdAsync(DriverId, CompanyId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DriverStoreDetailsDto>>(result.DriverStores);

            return Ok(dtoList);
        }
        [HttpGet("DriverAssignHistory/{UserId:guid}/{Role:int}")]
        public async Task<ActionResult> GetDriverAssignHistoryById(Guid UserId,int Role)
        {
            var result = await Repository.GetDriverAssignHistoryByIdAsync(UserId, Role);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.DriverStoreHistory);
        }

        [HttpGet("SystemOwner/{SystemOwnerId:guid}/{DriverId:guid}")]
        public async Task<ActionResult> GetDriverStoresBySystemOwnerId(Guid SystemOwnerId, Guid DriverId)
        {
            var result = await Repository.GetDriverStoresBySystemOwnerIdAsync(DriverId, SystemOwnerId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DriverStoreDetailsDto>>(result.DriverStores);

            return Ok(dtoList);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDeliveryTicket(DriverStoreDetailsDto DriverStoreDetailsDto)
        {
            var result = await Repository.UpdateDriverStoresAsync(DriverStoreDetailsDto);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

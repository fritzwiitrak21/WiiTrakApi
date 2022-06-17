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
    [Route("api/drivers")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly IDriverRepository Repository;

        public DriversController(IMapper mapper, IDriverRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetDriver")]
        public async Task<IActionResult> GetDriver(Guid id)
        {
            var result = await Repository.GetDriverByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<DriverDto>(result.Driver);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllDrivers()
        {
            var result = await Repository.GetAllDriversAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("Customer/{companyId:guid}")]
        public async Task<IActionResult> GetDriversByCustomerId(Guid companyId)
        {
            var result = await Repository.GetDriversByConditionAsync(x => x.CompanyId == companyId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("Company/{companyId:guid}")]
        [EnableQuery]
        public async Task<IActionResult> GetDriversByCompanyId(Guid companyId)
        {
            var result = await Repository.GetDriversByConditionAsync(x => x.CompanyId == companyId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("SystemOwner/{SystemOwnerId:guid}")]
        public async Task<IActionResult> GetDriversBySystemOwnerId(Guid SystemOwnerId)
        {
            var result = await Repository.GetDriversBySystemOwnerAsync(SystemOwnerId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("report/{id:guid}")]
        public async Task<IActionResult> GetDriverReport(Guid id)
        {
            var result = await Repository.GetDriverReportById(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverDto>> CreateDriver([FromBody] DriverDto driverCreation)
        {
            var driver = Mapper.Map<DriverModel>(driverCreation);
            driver.CreatedAt = DateTime.UtcNow;

            var createResult = await Repository.CreateDriverAsync(driver);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            var dto = Mapper.Map<DriverDto>(driver);
            return CreatedAtRoute(nameof(GetDriver), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDriver(Guid id, DriverDto driverUpdate)
        {
            var result = await Repository.GetDriverByIdAsync(id);
            if (!result.IsSuccess || result.Driver is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(driverUpdate, result.Driver);
            result.Driver.UpdatedAt = DateTime.UtcNow;
            var updateResult = await Repository.UpdateDriverAsync(result.Driver);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(Guid id)
        {
            var result = await Repository.DeleteDriverAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

    }
}

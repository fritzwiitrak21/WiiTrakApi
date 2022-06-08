/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IMapper _mapper;
        private readonly IDriverRepository _repository;

        public DriversController(IMapper mapper, IDriverRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetDriver")]
        public async Task<IActionResult> GetDriver(Guid id)
        {
            var result = await _repository.GetDriverByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<DriverDto>(result.Driver);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllDrivers()
        {
            var result = await _repository.GetAllDriversAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("Customer/{companyId:guid}")]
        public async Task<IActionResult> GetDriversByCustomerId(Guid companyId)
        {
            var result =
                await _repository.GetDriversByConditionAsync(x => x.CompanyId == companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("Company/{companyId:guid}")]
        [EnableQuery]
        public async Task<IActionResult> GetDriversByCompanyId(Guid companyId)
        {
            var result = await _repository
                .GetDriversByConditionAsync(x => x.CompanyId == companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("SystemOwner/{SystemOwnerId:guid}")]
        public async Task<IActionResult> GetDriversBySystemOwnerId(Guid SystemOwnerId)
        {
            var result = await _repository.GetDriversBySystemOwnerAsync(SystemOwnerId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("report/{id:guid}")]
        public async Task<IActionResult> GetDriverReport(Guid id)
        {
            var result = await _repository.GetDriverReportById(id);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverDto>> CreateDriver([FromBody] DriverCreationDto driverCreation)
        {
            var driver = _mapper.Map<DriverModel>(driverCreation);
            driver.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateDriverAsync(driver);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<DriverDto>(driver);
            return CreatedAtRoute(nameof(GetDriver), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDriver(Guid id, DriverUpdateDto driverUpdate)
        {
            var result = await _repository.GetDriverByIdAsync(id);

            if (!result.IsSuccess || result.Driver is null) return NotFound(result.ErrorMessage);
            _mapper.Map(driverUpdate, result.Driver);
            result.Driver.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateDriverAsync(result.Driver);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(Guid id)
        {
            var result = await _repository.DeleteDriverAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

    }
}

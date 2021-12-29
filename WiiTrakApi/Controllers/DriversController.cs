using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/drivers")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ILogger<DriversController> _logger;
        private readonly IMapper _mapper;
        private readonly IDriverRepository _repository;

        public DriversController(ILogger<DriversController> logger,
            IMapper mapper,
            IDriverRepository repository)
        {
            _logger = logger;
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

        [HttpGet("{companyId:guid}")]
        public async Task<IActionResult> GetDriversByCustomerId(Guid companyId)
        {
            var result = 
                await _repository.GetDriversByConditionAsync(x => x.CompanyId == companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
        }

        [HttpGet("Customer/{companyId:guid}")]
        [EnableQuery]
        public async Task<IActionResult> GetDriversByCompanyId(Guid companyId)
        {
            var result = await _repository
                .GetDriversByConditionAsync(x => x.CompanyId == companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverDto>>(result.Drivers);
            return Ok(dtoList);
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
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
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

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(Guid id)
        {
            var result = await _repository.DeleteDriverAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }

    }
}

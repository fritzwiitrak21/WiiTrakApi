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
    [Route("api/technicians")]
    [ApiController]
    public class TechniciansController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ITechnicianRepository Repository;

        public TechniciansController(IMapper mapper,
            ITechnicianRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetTechnician")]
        public async Task<IActionResult> GetTechnician(Guid id)
        {
            var result = await Repository.GetTechnicianByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<TechnicianDto>(result.Technician);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllTechnicians()
        {
            var result = await Repository.GetAllTechniciansAsync();

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<TechnicianDto>>(result.Technicians);
            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TechnicianDto>> CreateTechnician([FromBody] TechnicianCreationDto TechnicianCreation)
        {
            var technician = Mapper.Map<TechnicianModel>(TechnicianCreation);
            technician.CreatedAt = DateTime.UtcNow;

            var createResult = await Repository.CreateTechnicianAsync(technician);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = Mapper.Map<TechnicianDto>(technician);
            return CreatedAtRoute(nameof(GetTechnician), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTechnician(Guid id, TechnicianUpdateDto TechnicianUpdate)
        {
            var result = await Repository.GetTechnicianByIdAsync(id);

            if (!result.IsSuccess || result.Technician is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(TechnicianUpdate, result.Technician);
            result.Technician.UpdatedAt = DateTime.UtcNow;

            var updateResult = await Repository.UpdateTechnicianAsync(result.Technician);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnician(Guid id)
        {
            var result = await Repository.DeleteTechnicianAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

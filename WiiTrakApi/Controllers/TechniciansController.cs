using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/technicians")]
    [ApiController]
    public class TechniciansController : ControllerBase
    {
        private readonly ILogger<TechniciansController> _logger;
        private readonly IMapper _mapper;
        private readonly ITechnicianRepository _repository;

        public TechniciansController(ILogger<TechniciansController> logger,
            IMapper mapper,
            ITechnicianRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetTechnician")]
        public async Task<IActionResult> GetTechnician(Guid id)
        {
            var result = await _repository.GetTechnicianByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<TechnicianDto>(result.Technician);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllTechnicians()
        {
            var result = await _repository.GetAllTechniciansAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<TechnicianDto>>(result.Technicians);
            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TechnicianDto>> CreateTechnician([FromBody] TechnicianCreationDto technicianCreation)
        {
            var technician = _mapper.Map<TechnicianModel>(technicianCreation);
            technician.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateTechnicianAsync(technician);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
            }

            var dto = _mapper.Map<TechnicianDto>(technician);
            return CreatedAtRoute(nameof(GetTechnician), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTechnician(Guid id, TechnicianUpdateDto technicianUpdate)
        {
            var result = await _repository.GetTechnicianByIdAsync(id);

            if (!result.IsSuccess || result.Technician is null) return NotFound(result.ErrorMessage);
            _mapper.Map(technicianUpdate, result.Technician);
            result.Technician.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateTechnicianAsync(result.Technician);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnician(Guid id)
        {
            var result = await _repository.DeleteTechnicianAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }
    }
}

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
    [Route("api/corporates")]
    [ApiController]
    public class CorporateController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICorporateRepository _repository;

        public CorporateController(IMapper mapper, ICorporateRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetCorporate")]
        public async Task<IActionResult> GetCorporate(Guid id)
        {
            var result = await _repository.GetCorporateByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<CorporateDto>(result.Corporate);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCorporates()
        {
            var result = await _repository.GetAllCorporatesAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CorporateDto>>(result.Corporates);
            return Ok(dtoList);
        }


        [HttpGet("report/{id:guid}")]
        public async Task<IActionResult> GetCorporateReport(Guid id)
        {
            var result = await _repository.GetCorporateReportById(id);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        [HttpGet("company/{companyId:guid}")]
        public async Task<IActionResult> GetCorporatesByCompanyId(Guid companyId)
        {
            var result = await _repository.GetCorporatesByCompanyIdAsync(companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CorporateDto>>(result.Corporates);
            return Ok(dtoList);
        }

        [HttpGet("SystemOwner/{SystemOwnerId:guid}")]
        public async Task<IActionResult> GetCorporatesBySystemOwnerId(Guid SystemOwnerId)
        {
            var result = await _repository.GetCorporatesBySystemOwnerIdAsync(SystemOwnerId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CorporateDto>>(result.Corporates);
            return Ok(dtoList);
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{companyid:guid}/{roleid:int}")]
        public async Task<ActionResult<CorporateDto>> CreateCorporate(Guid CompanyId, int RoleId,[FromBody] CorporateCreationDto corporateCreation)
        {
            var corporate = _mapper.Map<CorporateModel>(corporateCreation);
            corporate.CreatedAt = DateTime.UtcNow;

           //TODO company id

            var createResult = await _repository.CreateCorporateAsync(corporate,CompanyId,RoleId);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<CorporateDto>(corporate);
            return CreatedAtRoute(nameof(GetCorporate), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCorporate(Guid id, CorporateUpdateDto corporateUpdate)
        {
            var result = await _repository.GetCorporateByIdAsync(id);

            if (!result.IsSuccess || result.Corporate is null) return NotFound(result.ErrorMessage);
            _mapper.Map(corporateUpdate, result.Corporate);
            result.Corporate.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateCorporateAsync(result.Corporate);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorporate(Guid id)
        {
            var result = await _repository.DeleteCorporateAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

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
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ICompanyRepository Repository;

        public CompaniesController(IMapper mapper,
            ICompanyRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetCompany")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var result = await Repository.GetCompanyByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<CompanyDto>(result.Company);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await Repository.GetAllCompaniesAsync();

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CompanyDto>>(result.Companies);
            return Ok(dtoList);
        }

        [HttpGet("Company/{companyId:guid}")]
        public async Task<IActionResult> GetChildCompanies(Guid companyId)
        {
            var result =
                await Repository.GetCompaniesByConditionAsync(x => x.ParentId == companyId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CompanyDto>>(result.Companies);
            return Ok(dtoList);
        }

        [HttpGet("ParentCompany/{subcompanyId:guid}")]
        public async Task<IActionResult> GetParentCompany(Guid subcompanyId)
        {
            var result = await Repository.GetParentCompanyAsync(subcompanyId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<CompanyDto>(result.Company);
            return Ok(dto);
        }

        [HttpGet("report/{id:guid}")]
        public async Task<IActionResult> GetCompanyReport(Guid id)
        {
            var result = await Repository.GetCompanyReportById(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        [HttpGet("corporate/{corporateId:guid}")]
        public async Task<IActionResult> GetCompaniesByCorporateId(Guid corporateId)
        {
            var result = await Repository.GetPrimaryCompaniesByCorporateIdAsync(corporateId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CompanyDto>>(result.Companies);
            return Ok(dtoList);
        }
        [HttpGet("systemowner/{systemownerId:guid}")]
        public async Task<IActionResult> GetCompaniesBySystemOwnerId(Guid systemownerId)
        {
            var result = await Repository.GetCompaniesBySystemOwnerId(systemownerId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CompanyDto>>(result.Companies);
            return Ok(dtoList);
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyCreationDto companyCreation)
        {
            var company = Mapper.Map<CompanyModel>(companyCreation);
            company.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateCompanyAsync(company);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = Mapper.Map<CompanyDto>(company);
            return CreatedAtRoute(nameof(GetCompany), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, CompanyUpdateDto companyUpdate)
        {
            var result = await Repository.GetCompanyByIdAsync(id);

            if (!result.IsSuccess || result.Company is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(companyUpdate, result.Company);
            result.Company.UpdatedAt = DateTime.UtcNow;

            var updateResult = await Repository.UpdateCompanyAsync(result.Company);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var result = await Repository.DeleteCompanyAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _repository;

        public CompaniesController(IMapper mapper,
            ICompanyRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetCompany")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var result = await _repository.GetCompanyByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<CompanyDto>(result.Company);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _repository.GetAllCompaniesAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CompanyDto>>(result.Companies);
            return Ok(dtoList);
        }

        [HttpGet("Company/{companyId:guid}")]
        public async Task<IActionResult> GetChildCompanies(Guid companyId)
        {
            var result =
                await _repository.GetCompaniesByConditionAsync(x => x.ParentId == companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CompanyDto>>(result.Companies);
            return Ok(dtoList);
        }

        [HttpGet("ParentCompany/{subcompanyId:guid}")]
        public async Task<IActionResult> GetParentCompany(Guid subcompanyId)
        {
            var result =
                await _repository.GetParentCompanyAsync(subcompanyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<CompanyDto>(result.Company);
            return Ok(dto);
        }

        [HttpGet("report/{id:guid}")]
        public async Task<IActionResult> GetCompanyReport(Guid id)
        {
            var result = await _repository.GetCompanyReportById(id);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        [HttpGet("corporate/{corporateId:guid}")]
        public async Task<IActionResult> GetCompaniesByCorporateId(Guid corporateId)
        {
            var result = await _repository.GetPrimaryCompaniesByCorporateIdAsync(corporateId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CompanyDto>>(result.Companies);
            return Ok(dtoList);
        }
        [HttpGet("systemowner/{systemownerId:guid}")]
        public async Task<IActionResult> GetCompaniesBySystemOwnerId(Guid systemownerId)
        {
            var result = await _repository.GetCompaniesBySystemOwnerId(systemownerId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CompanyDto>>(result.Companies);
            return Ok(dtoList);
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyCreationDto companyCreation)
        {
            var company = _mapper.Map<CompanyModel>(companyCreation);
            company.CreatedAt = DateTime.UtcNow;
            var createResult = await _repository.CreateCompanyAsync(company);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<CompanyDto>(company);
            return CreatedAtRoute(nameof(GetCompany), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, CompanyUpdateDto companyUpdate)
        {
            var result = await _repository.GetCompanyByIdAsync(id);

            if (!result.IsSuccess || result.Company is null) return NotFound(result.ErrorMessage);
            _mapper.Map(companyUpdate, result.Company);
            result.Company.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateCompanyAsync(result.Company);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var result = await _repository.DeleteCompanyAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

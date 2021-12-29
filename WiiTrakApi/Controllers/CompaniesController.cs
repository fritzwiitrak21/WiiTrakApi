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
        private ILogger<CompaniesController> _logger;
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _repository;

        public CompaniesController(ILogger<CompaniesController> logger,
            IMapper mapper,
            ICompanyRepository repository)
        {
            _logger = logger;
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

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyCreationDto companyCreation)
        {
            var company = _mapper.Map<CompanyModel>(companyCreation);
            company.CreatedAt = DateTime.UtcNow;

            //
            //
            // TODO get sysowner id from db
            //
            //
            company.SystemOwnerId = Guid.Parse("b3109973-ff0a-4f51-e8eb-08d9c2ef5353");

            var createResult = await _repository.CreateCompanyAsync(company);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
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

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            var result = await _repository.DeleteCompanyAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }
    }
}

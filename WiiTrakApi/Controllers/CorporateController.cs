using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<CorporateController> _logger;
        private readonly IMapper _mapper;
        private readonly ICorporateRepository _repository;

        public CorporateController(ILogger<CorporateController> logger, IMapper mapper, ICorporateRepository repository)
        {
            _logger = logger;
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
            var result = await _repository.GetCorporatesByCompanyId(companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CorporateDto>>(result.Corporates);
            return Ok(dtoList);
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CorporateDto>> CreateCorporate([FromBody] CorporateCreationDto corporateCreation)
        {
            var corporate = _mapper.Map<CorporateModel>(corporateCreation);
            corporate.CreatedAt = DateTime.UtcNow;

           //TODO company id

            var createResult = await _repository.CreateCorporateAsync(corporate);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
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

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorporate(Guid id)
        {
            var result = await _repository.DeleteCorporateAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }
    }
}

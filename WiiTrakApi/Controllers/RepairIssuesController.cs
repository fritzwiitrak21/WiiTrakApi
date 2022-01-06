using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/repairissues")]
    [ApiController]
    public class RepairIssuesController : ControllerBase
    {
        private readonly ILogger<RepairIssuesController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepairIssueRepository _repository;

        public RepairIssuesController(ILogger<RepairIssuesController> logger, IMapper mapper, IRepairIssueRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetRepairIssue")]
        public async Task<IActionResult> GetRepairIssue(Guid id)
        {
            var result = await _repository.GetRepairIssueByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<AssetDto>(result.RepairIssue);
            return Ok(dto);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRepairIssues()
        {
            var result = await _repository.GetAllRepairIssuesAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<RepairIssueDto>>(result.RepairIssues);
            return Ok(dtoList);
        }

        [HttpPost]
        public async Task<ActionResult<RepairIssueDto>> CreateRepairIssue([FromBody] RepairIssueCreationDto repairIssueCreation)
        {
            var repairIssue = _mapper.Map<RepairIssueModel>(repairIssueCreation);
            repairIssue.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateRepairIssueAsync(repairIssue);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
            }

            var dto = _mapper.Map<RepairIssueDto>(repairIssue);
            return CreatedAtRoute(nameof(GetRepairIssue), new { id = dto.Id }, dto);
        }

        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRepairIssue(Guid id, RepairIssueUpdateDto repairIssueUpdate)
        {
            var result = await _repository.GetRepairIssueByIdAsync(id);

            if (!result.IsSuccess || result.RepairIssue is null) return NotFound(result.ErrorMessage);
            _mapper.Map(repairIssueUpdate, result.RepairIssue);
            result.RepairIssue.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateRepairIssueAsync(result.RepairIssue);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairIssue(Guid id)
        {
            var result = await _repository.DeleteRepairIssueAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }
    }
}

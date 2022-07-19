/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/repairissues")]
    [ApiController]
    public class RepairIssuesController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly IRepairIssueRepository Repository;

        public RepairIssuesController(IMapper mapper, IRepairIssueRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetRepairIssue")]
        public async Task<IActionResult> GetRepairIssue(Guid id)
        {
            var result = await Repository.GetRepairIssueByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<CartDto>(result.RepairIssue);
            return Ok(dto);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllRepairIssues()
        {
            var result = await Repository.GetAllRepairIssuesAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<RepairIssueDto>>(result.RepairIssues);
            return Ok(dtoList);
        }

        [HttpPost]
        public async Task<ActionResult<RepairIssueDto>> CreateRepairIssue([FromBody] RepairIssueDto RepairIssueCreation)
        {
            var repairIssue = Mapper.Map<RepairIssueModel>(RepairIssueCreation);
            repairIssue.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateRepairIssueAsync(repairIssue);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            var dto = Mapper.Map<RepairIssueDto>(repairIssue);
            return CreatedAtRoute(nameof(GetRepairIssue), new { id = dto.Id }, dto);
        }

        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRepairIssue(Guid id, RepairIssueDto RepairIssueUpdate)
        {
            var result = await Repository.GetRepairIssueByIdAsync(id);
            if (!result.IsSuccess || result.RepairIssue is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(RepairIssueUpdate, result.RepairIssue);
            result.RepairIssue.UpdatedAt = DateTime.UtcNow;
            var updateResult = await Repository.UpdateRepairIssueAsync(result.RepairIssue);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairIssue(Guid id)
        {
            var result = await Repository.DeleteRepairIssueAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

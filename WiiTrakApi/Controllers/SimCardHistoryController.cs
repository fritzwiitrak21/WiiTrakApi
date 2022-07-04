/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using AutoMapper;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/simcardhistory")]
    [ApiController]
    public class SimCardHistoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISimCardHistoryRepository _repository;

        public SimCardHistoryController(IMapper mapper, ISimCardHistoryRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        [EnableQuery]
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetSimCardHistoryById(Guid Id)
        {
            var result = await _repository.GetSimCardHistoryByIdAsync(Id);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            var dtoList = _mapper.Map<List<SimCardHistoryDto>>(result.SimCardHistory);

            return Ok(dtoList);
        }
        [HttpPost]
        public async Task<ActionResult<SimCardHistoryDto>> CreateSimCardHistory([FromBody] SimCardHistoryDto SimCardHistoryCreation)
        {
            var SimCardHistory = _mapper.Map<SimCardHistoryModel>(SimCardHistoryCreation);
            SimCardHistory.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateSimCardHistoryAsync(SimCardHistory);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<SimCardHistoryDto>(SimCardHistory);
            return CreatedAtRoute(nameof(GetSimCardHistoryById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSimCardHistory(Guid id, SimCardHistoryDto SimCardHistoryUpdate)
        {
            var result = await _repository.GetSimCardHistoryByIdAsync(id);

            if (!result.IsSuccess || result.SimCardHistory is null)
            {
                return NotFound(result.ErrorMessage);
            }
            _mapper.Map(SimCardHistoryUpdate, result.SimCardHistory);
            result.SimCardHistory.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateSimCardHistoryAsync(result.SimCardHistory);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

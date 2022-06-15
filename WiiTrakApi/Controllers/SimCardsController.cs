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
    [Route("api/simcards")]
    [ApiController]
    public class SimCardsController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ISimCardsRepository Repository;

        public SimCardsController(IMapper mapper, ISimCardsRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllSimCardDetails()
        {
            var result = await Repository.GetAllSimCardDetailsAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtolist = Mapper.Map<List<SimCardsDto>>(result.SimCardList);
            return Ok(dtolist);
        }
        [HttpGet("{id:guid}", Name = "GetSimCard")]
        public async Task<IActionResult> GetSimCardById(Guid id)
        {
            var result = await Repository.GetSimCardByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<SimCardsDto>(result.SimCardList);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult<SimCardsDto>> CreateSimCard([FromBody] SimCardsDto SimCardCreation)
        {
            var Simcard = Mapper.Map<SimCardModel>(SimCardCreation);
            Simcard.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateSimCardAsync(Simcard);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = Mapper.Map<SimCardsDto>(Simcard);
            return CreatedAtRoute(nameof(GetSimCardById), new { id = dto.Id }, dto);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSimCard(Guid id, SimCardsDto SimCardUpdate)
        {
            var result = await Repository.GetSimCardByIdAsync(id);

            if (!result.IsSuccess || result.SimCardList is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(SimCardUpdate, result.SimCardList);
            result.SimCardList.UpdatedAt = DateTime.UtcNow;

            var updateResult = await Repository.UpdateSimCardAsync(result.SimCardList);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

    }
}

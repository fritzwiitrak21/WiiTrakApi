using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;
        private readonly ISimCardsRepository _repository;

        public SimCardsController(IMapper mapper, ISimCardsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllSimCardDetails()
        {
            var result = await _repository.GetAllSimCardDetailsAsync();
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtolist = _mapper.Map<List<SimCardsDto>>(result.SimCardList);
            return Ok(dtolist);
        }
        [HttpGet("{id:guid}", Name = "GetSimCard")]
        public async Task<IActionResult> GetSimCardById(Guid id)
        {
            var result = await _repository.GetSimCardByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<SimCardsDto>(result.SimCardList);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult<SimCardsDto>> CreateSimCard([FromBody] SimCardCreationDto SimCardCreation)
        {
            var Simcard = _mapper.Map<SimCardModel>(SimCardCreation);
            Simcard.CreatedAt = DateTime.UtcNow;
            var createResult = await _repository.CreateSimCardAsync(Simcard);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<SimCardsDto>(Simcard);
            return CreatedAtRoute(nameof(GetSimCardById), new { id = dto.Id }, dto);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSimCard(Guid id, SimCardUpdateDto SimCardUpdate)
        {
            var result = await _repository.GetSimCardByIdAsync(id);

            if (!result.IsSuccess || result.SimCardList is null) return NotFound(result.ErrorMessage);
            _mapper.Map(SimCardUpdate, result.SimCardList);
            result.SimCardList.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateSimCardAsync(result.SimCardList);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

    }
}

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
    [Route("api/countycode")]
    [ApiController]
    public class CountyCodeController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ICountyCodeRepository Repository;

        public CountyCodeController( IMapper mapper, ICountyCodeRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }
        [HttpGet("{id:guid}", Name = "GetCountyCode")]
        public async Task<IActionResult> GetCountyCodeById(Guid id)
        {
            var result = await Repository.GetCountyCodeByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<CountyCodeDto>(result.CountyCode);
            return Ok(dto);
        }
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetCountyList()
        {
            var result = await Repository.GetCountyListAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtolist = Mapper.Map<List<CountyCodeDto>>(result.CountyList);
            return Ok(dtolist);
        }
        [HttpPost]
        public async Task<ActionResult<CountyCodeDto>> CreateCountyCode([FromBody] CountyCodeDto CountyCreation)
        {
            var countycode = Mapper.Map<CountyCodeModel>(CountyCreation);
            countycode.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateCountyCodeAsync(countycode);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            var dto = Mapper.Map<CountyCodeDto>(countycode);
            return CreatedAtRoute(nameof(GetCountyCodeById), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStore(Guid id, CountyCodeDto CountyUpdate)
        {
            var result = await Repository.GetCountyCodeByIdAsync(id);
            if (!result.IsSuccess || result.CountyCode is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(CountyUpdate, result.CountyCode);
            result.CountyCode.UpdatedAt = DateTime.UtcNow;
            var updateResult = await Repository.UpdateCountyCodeAsync(result.CountyCode);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

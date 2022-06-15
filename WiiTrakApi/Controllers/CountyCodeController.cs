/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
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
    }
}

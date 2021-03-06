/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/tracksolid")]
    [ApiController]
    public class TrackSolidController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ITrackSolidRepository Repository;
        public TrackSolidController(IMapper mapper, ITrackSolidRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCoordinatesOfDevices()
        {
            var updateResult = await Repository.GetDataFromTrackSolidAsync();
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
        }
    }

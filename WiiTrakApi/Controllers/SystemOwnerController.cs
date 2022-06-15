/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/systemowner")]
    [ApiController]
    public class SystemOwnerController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ISystemOwnerRepository Repository;
        public SystemOwnerController( IMapper mapper, ISystemOwnerRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetSystemOwnerById(Guid Id)
        {
            var result = await Repository.GetSystemOwnerByIdAsync(Id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.SystemOwner);
        }

        [HttpGet("{EmailId}")]
        public async Task<IActionResult> CheckEmailId(string EmailId)
        {
            var result = await Repository.CheckEmailIdAsync(EmailId);
            if (!result.IsExists)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.ErrorMessage);
        }
    }
}

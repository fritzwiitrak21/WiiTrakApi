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
        private readonly IMapper _mapper;
        private readonly ISystemOwnerRepository _repository;
        public SystemOwnerController( IMapper mapper, ISystemOwnerRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetSystemOwnerById(Guid Id)
        {
            var result = await _repository.GetSystemOwnerByIdAsync(Id);
            if(!result.IsSuccess) return NotFound(result.ErrorMessage);
            return Ok(result.SystemOwner);


        }
        [HttpGet("{EmailId}")]
        public async Task<IActionResult> CheckEmailId(string EmailId)
        {
            var result = await _repository.CheckEmailIdAsync(EmailId);
            if (!result.IsExists) return NotFound(result.ErrorMessage);
            return Ok(result.ErrorMessage);
        }

    }


       

    }

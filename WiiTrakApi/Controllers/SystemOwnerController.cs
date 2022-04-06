using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Query;
using AutoMapper;
using WiiTrakApi.DTOs;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/systemowner")]
    [ApiController]
    public class SystemOwnerController : ControllerBase
    {
        private readonly ILogger<SystemOwnerController> _logger;
        private readonly IMapper _mapper;
        private readonly ISystemOwnerRepository _repository;
        public SystemOwnerController(ILogger<SystemOwnerController> logger, IMapper mapper, ISystemOwnerRepository repository)
        {
            _logger = logger;
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

    }


       

    }

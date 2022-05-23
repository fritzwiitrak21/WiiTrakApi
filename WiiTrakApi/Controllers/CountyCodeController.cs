using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;
        private readonly ICountyCodeRepository _repository;

        public CountyCodeController( IMapper mapper, ICountyCodeRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetCountyList()
        {
            var result = await _repository.GetCountyListAsync();
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtolist = _mapper.Map<List<CountyCodeDto>>(result.CountyList);
            return Ok(dtolist);
        }
    }
}

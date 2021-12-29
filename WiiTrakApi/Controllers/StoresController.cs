using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly ILogger<StoresController> _logger;
        private readonly IMapper _mapper;
        private readonly IStoreRepository _repository;

        public StoresController(ILogger<StoresController> logger,
            IMapper mapper,
            IStoreRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetStore")]
        public async Task<IActionResult> GetStore(Guid id)
        {
            var result = await _repository.GetStoreByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<StoreDto>(result.Store);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllStores()
        {
            var result = await _repository.GetAllStoresAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("ServiceProvider/{serviceProviderId:guid}")]
        [EnableQuery]
        public async Task<IActionResult> GetStoresByServiceProviderId(Guid serviceProviderId)
        {
            var result = await _repository
                .GetStoresByConditionAsync(x => x.ServiceProviderId == serviceProviderId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StoreDto>> CreateStore([FromBody] StoreCreationDto storeCreation)
        {
            var store = _mapper.Map<StoreModel>(storeCreation);
            store.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateStoreAsync(store);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
            }

            var dto = _mapper.Map<StoreDto>(store);
            return CreatedAtRoute(nameof(GetStore), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStore(Guid id, StoreUpdateDto storeUpdate)
        {
            var result = await _repository.GetStoreByIdAsync(id);

            if (!result.IsSuccess || result.Store is null) return NotFound(result.ErrorMessage);
            _mapper.Map(storeUpdate, result.Store);
            result.Store.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateStoreAsync(result.Store);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(Guid id)
        {
            var result = await _repository.DeleteStoreAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }
    }
}

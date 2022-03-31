using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using Microsoft.AspNetCore.OData.Query;

namespace WiiTrakApi.Controllers
{
    [Route("api/driverstores")]
    [ApiController]
    public class DriverStoresController : ControllerBase
    {
        private readonly ILogger<DriverStoresController> _logger;
        private readonly IMapper _mapper;
        private readonly IDriverStoresRepository _repository;
      

        public DriverStoresController(ILogger<DriverStoresController> logger,
           IMapper mapper,
           IDriverStoresRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }
        
        [HttpGet("GetDriverStoresByCompanyId")]
        public async Task<ActionResult> GetDriverStoresByCompanyId(Guid CompanyId,Guid DriverId)
        {
            var result = await _repository.GetDriverStoresByCompanyIdAsync(DriverId,CompanyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverStoreDetailsDto>>(result.DriverStores);

            return Ok(dtoList);
        }

        [HttpGet("GetDriverStoresBySystemOwnerId")]
        public async Task<ActionResult> GetDriverStoresBySystemOwnerId(Guid SystemOwnerId,Guid DriverId)
        {
            var result = await _repository.GetDriverStoresBySystemOwnerIdAsync(DriverId, SystemOwnerId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DriverStoreDetailsDto>>(result.DriverStores);

            return Ok(dtoList);
        }

        [HttpPut("{DriverId:guid}")]
        public async Task<IActionResult> UpdateDeliveryTicket(Guid DriverId,Guid StoreId,bool IsActive)
        {
            var result = await _repository.UpdateDriverStoresAsync(DriverId, StoreId, IsActive);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }
    }
}

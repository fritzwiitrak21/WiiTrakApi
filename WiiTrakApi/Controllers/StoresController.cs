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
    [Route("api/stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly IStoreRepository Repository;

        public StoresController(IMapper mapper,
            IStoreRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetStore")]
        public async Task<IActionResult> GetStore(Guid id)
        {
            var result = await Repository.GetStoreByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<StoreDto>(result.Store);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllStores()
        {
            var result = await Repository.GetAllStoresAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("ServiceProvider/{ServiceProviderId:guid}")]
        [EnableQuery]
        public async Task<IActionResult> GetStoresByServiceProviderId(Guid ServiceProviderId)
        {
            var result = await Repository
                .GetStoresByConditionAsync(x => x.ServiceProviderId == ServiceProviderId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("Corporate/{CorporateId:guid}")]
        public async Task<IActionResult> GetStoresByCorporateId(Guid CorporateId)
        {
            var result = await Repository.GetStoresByCorporateId(CorporateId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("Company/{CompanyId:guid}")]
        public async Task<IActionResult> GetStoresByCompanyId(Guid CompanyId)
        {
            var result = await Repository.GetStoresByCompanyId(CompanyId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }
        [HttpGet("Technician/{TechnicianId:guid}")]
        public async Task<IActionResult> GetStoresByTechnicianId(Guid TechnicianId)
        {
            var result = await Repository.GetStoresByTechnicianId(TechnicianId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }
        [HttpGet("Systemowner/{SystemownerId:guid}")]
        public async Task<IActionResult> GetStoresBySystemOwnerId(Guid SystemownerId)
        {
            var result = await Repository.GetStoresBySystemOwnerId(SystemownerId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("Driver/{DriverId:guid}")]
        public async Task<IActionResult> GetStoresByDriverId(Guid DriverId)
        {
            var result = await Repository.GetStoresByDriverId(DriverId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            dtoList = dtoList.OrderBy(o => o.StoreName).ThenBy(p => p.StoreNumber).ToList();
            return Ok(dtoList);
        }
        [HttpGet("report/{id:guid}")]
        public async Task<IActionResult> GetStoreReport(Guid id)
        {
            var result = await Repository.GetStoreReportById(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        [HttpGet("report/driver/{DriverId:guid}")]
        public async Task<IActionResult> GetAllStoreReportByDriver(Guid DriverId)
        {
            var result = await Repository.GetAllStoreReportByDriverId(DriverId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        [HttpGet("report/corporate/{CorporateId:guid}")]
        public async Task<IActionResult> GetAllStoreReportByCorporate(Guid CorporateId)
        {
            var result = await Repository.GetAllStoreReportByCorporateId(CorporateId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        [HttpGet("report/company/{CompanyId:guid}")]
        public async Task<IActionResult> GetAllStoreReportByCompany(Guid CompanyId)
        {
            var result = await Repository.GetAllStoreReportByCompanyId(CompanyId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }
        [HttpGet("StoreUpdateHistory/{UserId:guid}/{Role:int}")]
        public async Task<ActionResult> GetStoreUpdateHistoryById(Guid UserId, int Role)
        {
            var result = await Repository.GetStoreUpdateHistoryByIdAsync(UserId, Role);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.StoreUpdateHistory);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StoreDto>> CreateStore([FromBody] StoreDto StoreCreation)
        {
            // TODO
            // company Id to one-to-many
            // Corporate Id to one-to-many
            var store = Mapper.Map<StoreModel>(StoreCreation);
            store.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateStoreAsync(store,(Guid)StoreCreation.CreatedBy);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            var dto = Mapper.Map<StoreDto>(store);
            return CreatedAtRoute(nameof(GetStore), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStore(Guid id, StoreDto StoreUpdate)
        {
            var result = await Repository.GetStoreByIdAsync(id);
            if (!result.IsSuccess || result.Store is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(StoreUpdate, result.Store);
            result.Store.UpdatedAt = DateTime.UtcNow;
            var updateResult = await Repository.UpdateStoreAsync(result.Store,(Guid)StoreUpdate.CreatedBy);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStoreFenceCoords(StoreDto StoreUpdate)
        {
          
            var updateResult = await Repository.UpdateStoreFenceCoordsAsync(StoreUpdate);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(Guid id)
        {
            var result = await Repository.DeleteStoreAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
        
    }
}

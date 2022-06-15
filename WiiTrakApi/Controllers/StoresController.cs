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

        [HttpGet("ServiceProvider/{serviceProviderId:guid}")]
        [EnableQuery]
        public async Task<IActionResult> GetStoresByServiceProviderId(Guid serviceProviderId)
        {
            var result = await Repository
                .GetStoresByConditionAsync(x => x.ServiceProviderId == serviceProviderId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("Corporate/{corporateId:guid}")]
        public async Task<IActionResult> GetStoresByCorporateId(Guid corporateId)
        {
            var result = await Repository.GetStoresByCorporateId(corporateId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("Company/{companyId:guid}")]
        public async Task<IActionResult> GetStoresByCompanyId(Guid companyId)
        {
            var result = await Repository.GetStoresByCompanyId(companyId);
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

        [HttpGet("Driver/{driverId:guid}")]
        public async Task<IActionResult> GetStoresByDriverId(Guid driverId)
        {
            var result = await Repository.GetStoresByDriverId(driverId);
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

        [HttpGet("report/driver/{driverId:guid}")]
        public async Task<IActionResult> GetAllStoreReportByDriver(Guid driverId)
        {
            var result = await Repository.GetAllStoreReportByDriverId(driverId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        [HttpGet("report/corporate/{corporateId:guid}")]
        public async Task<IActionResult> GetAllStoreReportByCorporate(Guid corporateId)
        {
            var result = await Repository.GetAllStoreReportByCorporateId(corporateId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        [HttpGet("report/company/{companyId:guid}")]
        public async Task<IActionResult> GetAllStoreReportByCompany(Guid companyId)
        {
            var result = await Repository.GetAllStoreReportByCompanyId(companyId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var reportDto = result.Report;
            return Ok(reportDto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StoreDto>> CreateStore([FromBody] StoreCreationDto storeCreation)
        {
            // TODO
            // company Id to one-to-many
            // Corporate Id to one-to-many
            var store = Mapper.Map<StoreModel>(storeCreation);
            store.CreatedAt = DateTime.UtcNow;
            var createResult = await Repository.CreateStoreAsync(store);
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
        public async Task<IActionResult> UpdateStore(Guid id, StoreUpdateDto storeUpdate)
        {
            var result = await Repository.GetStoreByIdAsync(id);
            if (!result.IsSuccess || result.Store is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(storeUpdate, result.Store);
            result.Store.UpdatedAt = DateTime.UtcNow;
            var updateResult = await Repository.UpdateStoreAsync(result.Store);
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

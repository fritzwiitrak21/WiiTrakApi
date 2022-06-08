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
        private readonly IMapper _mapper;
        private readonly IStoreRepository _repository;

        public StoresController(IMapper mapper,
            IStoreRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetStore")]
        public async Task<IActionResult> GetStore(Guid id)
        {
            var result = await _repository.GetStoreByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = _mapper.Map<StoreDto>(result.Store);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllStores()
        {
            var result = await _repository.GetAllStoresAsync();

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("ServiceProvider/{serviceProviderId:guid}")]
        [EnableQuery]
        public async Task<IActionResult> GetStoresByServiceProviderId(Guid serviceProviderId)
        {
            var result = await _repository
                .GetStoresByConditionAsync(x => x.ServiceProviderId == serviceProviderId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("Corporate/{corporateId:guid}")]
        public async Task<IActionResult> GetStoresByCorporateId(Guid corporateId)
        {
            var result = await _repository.GetStoresByCorporateId(corporateId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("Company/{companyId:guid}")]
        public async Task<IActionResult> GetStoresByCompanyId(Guid companyId)
        {
            var result = await _repository.GetStoresByCompanyId(companyId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }
        [HttpGet("Systemowner/{SystemownerId:guid}")]
        public async Task<IActionResult> GetStoresBySystemOwnerId(Guid SystemownerId)
        {
            var result = await _repository.GetStoresBySystemOwnerId(SystemownerId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<StoreDto>>(result.Stores);
            return Ok(dtoList);
        }

        [HttpGet("Driver/{driverId:guid}")]
        public async Task<IActionResult> GetStoresByDriverId(Guid driverId)
        {
            var result = await _repository.GetStoresByDriverId(driverId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<StoreDto>>(result.Stores);
            dtoList = dtoList.OrderBy(o => o.StoreName).ThenBy(p => p.StoreNumber).ToList();
            return Ok(dtoList);
        }
        [HttpGet("report/{id:guid}")]
        public async Task<IActionResult> GetStoreReport(Guid id)
        {
            var result = await _repository.GetStoreReportById(id);

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
            var result = await _repository.GetAllStoreReportByDriverId(driverId);

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
            var result = await _repository.GetAllStoreReportByCorporateId(corporateId);

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
            var result = await _repository.GetAllStoreReportByCompanyId(companyId);

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

            var store = _mapper.Map<StoreModel>(storeCreation);
            store.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateStoreAsync(store);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<StoreDto>(store);
            return CreatedAtRoute(nameof(GetStore), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStore(Guid id, StoreUpdateDto storeUpdate)
        {
            var result = await _repository.GetStoreByIdAsync(id);

            if (!result.IsSuccess || result.Store is null)
            {
                return NotFound(result.ErrorMessage);
            }
            _mapper.Map(storeUpdate, result.Store);
            result.Store.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateStoreAsync(result.Store);
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
            var result = await _repository.DeleteStoreAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

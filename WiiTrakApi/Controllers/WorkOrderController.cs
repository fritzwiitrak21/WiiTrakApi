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
    [Route("api/workorders")]
    [ApiController]
    public class WorkOrderController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly IWorkOrderRepository Repository;
        private readonly IDriverRepository DriverRepository;
        private readonly IStoreRepository StoreRepository;

        public WorkOrderController(IMapper mapper, IWorkOrderRepository repository, IDriverRepository driverrepository, IStoreRepository storerepository)
        {
            Mapper = mapper;
            Repository = repository;
            DriverRepository = driverrepository;
            StoreRepository = storerepository;
        }

        [HttpGet("{id:guid}", Name = "GetWorkOrder")]
        public async Task<IActionResult> GetWorkOrder(Guid id)
        {
            var result = await Repository.GetWorkOrderByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = Mapper.Map<WorkOrderDto>(result.WorkOrder);

            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllWorkOrders()
        {
            var result = await Repository.GetAllWorkOrdersAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = Mapper.Map<List<WorkOrderDto>>(result.WorkOrders);


            return Ok(dtoList);
        }

        //[HttpGet("Driver/{driverId:guid}")]
        //public async Task<IActionResult> GetAllWorkOrdersByDriverId(Guid driverId)
        //{
        //    var result = await _repository
        //        .GetWorkOrdersByConditionAsync(x => x.DriverId == driverId);

        //    if (!result.IsSuccess) return NotFound(result.ErrorMessage);
        //    var dtoList = _mapper.Map<List<WorkOrderDto>>(result.WorkOrders);

        //    // get store name and number
        //    foreach (var dto in dtoList)
        //    {
        //        var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
        //        var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

        //        dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
        //        dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
        //        dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
        //    }

        //    return Ok(dtoList);
        //}

        //[HttpGet("Store/{storeId:guid}")]
        //public async Task<IActionResult> GetAllWorkOrdersByStoreId(Guid storeId)
        //{
        //    var result = await _repository
        //        .GetWorkOrdersByConditionAsync(x => x.StoreId == storeId);

        //    if (!result.IsSuccess) return NotFound(result.ErrorMessage);
        //    var dtoList = _mapper.Map<List<WorkOrderDto>>(result.WorkOrders);

        //    // get store name and number
        //    foreach (var dto in dtoList)
        //    {
        //        var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
        //        var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

        //        dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
        //        dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
        //        dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
        //    }

        //    return Ok(dtoList);
        //}

        //[HttpGet("ServiceProvider/{serviceProviderId:guid}")]
        //public async Task<IActionResult> GetAllWorkOrdersByServiceProviderId(Guid serviceProviderId)
        //{
        //    var result = await _repository
        //        .GetWorkOrdersByConditionAsync(x => x.ServiceProviderId == serviceProviderId);

        //    if (!result.IsSuccess) return NotFound(result.ErrorMessage);
        //    var dtoList = _mapper.Map<List<WorkOrderDto>>(result.WorkOrders);

        //    // get store name and number
        //    foreach (var dto in dtoList)
        //    {
        //        var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
        //        var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

        //        dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
        //        dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
        //        dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
        //    }

        //    return Ok(dtoList);
        //}


        [HttpPost]
        public async Task<ActionResult<WorkOrderDto>> CreateWorkOrder([FromBody] WorkOrderCreationDto workOrderCreation)
        {
            var workOrder = Mapper.Map<WorkOrderModel>(workOrderCreation);
            workOrder.CreatedAt = DateTime.UtcNow;

            var workOrderNumberResult = await Repository.GetWorkOrderNumberAsync();
            workOrder.WorkOrderNumber = workOrderNumberResult.WorkOrderNumber;

            var createResult = await Repository.CreateWorkOrderAsync(workOrder);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = Mapper.Map<WorkOrderDto>(workOrder);
            return CreatedAtRoute(nameof(GetWorkOrder), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateWorkOrder(Guid id, WorkOrderUpdateDto workOrderUpdate)
        {
            var result = await Repository.GetWorkOrderByIdAsync(id);

            if (!result.IsSuccess || result.WorkOrder is null) return NotFound(result.ErrorMessage);
            Mapper.Map(workOrderUpdate, result.WorkOrder);
            result.WorkOrder.UpdatedAt = DateTime.UtcNow;

            var updateResult = await Repository.UpdateWorkOrderAsync(result.WorkOrder);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkOrder(Guid id)
        {
            var result = await Repository.DeleteWorkOrderAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

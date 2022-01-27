using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/workorders")]
    [ApiController]
    public class WorkOrderController : ControllerBase
    {
        private readonly ILogger<WorkOrderController> _logger;
        private readonly IMapper _mapper;
        private readonly IWorkOrderRepository _repository;
        private readonly IDriverRepository _driverRepository;
        private readonly IStoreRepository _storeRepository;

        public WorkOrderController(ILogger<WorkOrderController> logger, IMapper mapper, IWorkOrderRepository repository, IDriverRepository driverRepository, IStoreRepository storeRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _driverRepository = driverRepository;
            _storeRepository = storeRepository;
        }

        [HttpGet("{id:guid}", Name = "GetWorkOrder")]
        public async Task<IActionResult> GetWorkOrder(Guid id)
        {
            var result = await _repository.GetWorkOrderByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<WorkOrderDto>(result.WorkOrder);

            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllWorkOrders()
        {
            var result = await _repository.GetAllWorkOrdersAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<WorkOrderDto>>(result.WorkOrders);


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
            var workOrder = _mapper.Map<WorkOrderModel>(workOrderCreation);
            workOrder.CreatedAt = DateTime.UtcNow;

            var workOrderNumberResult = await _repository.GetWorkOrderNumberAsync();
            workOrder.WorkOrderNumber = workOrderNumberResult.WorkOrderNumber;

            var createResult = await _repository.CreateWorkOrderAsync(workOrder);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
            }

            var dto = _mapper.Map<WorkOrderDto>(workOrder);
            return CreatedAtRoute(nameof(GetWorkOrder), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateWorkOrder(Guid id, WorkOrderUpdateDto workOrderUpdate)
        {
            var result = await _repository.GetWorkOrderByIdAsync(id);

            if (!result.IsSuccess || result.WorkOrder is null) return NotFound(result.ErrorMessage);
            _mapper.Map(workOrderUpdate, result.WorkOrder);
            result.WorkOrder.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateWorkOrderAsync(result.WorkOrder);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkOrder(Guid id)
        {
            var result = await _repository.DeleteWorkOrderAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }
    }
}

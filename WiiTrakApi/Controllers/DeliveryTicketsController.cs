using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/deliverytickets")]
    [ApiController]
    public class DeliveryTicketsController : ControllerBase
    {
        private readonly ILogger<DeliveryTicketsController> _logger;
        private readonly IMapper _mapper;
        private readonly IDeliveryTicketRepository _repository;
        private readonly IDriverRepository _driverRepository;
        private readonly IStoreRepository _storeRepository;

        public DeliveryTicketsController(ILogger<DeliveryTicketsController> logger, IMapper mapper, IDeliveryTicketRepository repository, IDriverRepository driverRepository, IStoreRepository storeRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _driverRepository = driverRepository;
            _storeRepository = storeRepository;
        }
        

        [HttpGet("{id:guid}", Name = "GetDeliveryTicket")]
        public async Task<IActionResult> GetDeliveryTicket(Guid id)
        {
            var result = await _repository.GetDeliveryTicketByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<DeliveryTicketDto>(result.DeliveryTicket);

            // get store name and number
            var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
            var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

            dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
            dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
            dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";

            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllDeliveryTickets()
        {
            var result = await _repository.GetAllDeliveryTicketsAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

                dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
                dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
                dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
            }

            return Ok(dtoList);
        }

        [HttpGet("Driver/{driverId:guid}")]
        public async Task<IActionResult> GetAllDeliveryTicketsByDriverId(Guid driverId)
        {
            var result = await _repository
                .GetDeliveryTicketsByConditionAsync(x => x.DriverId == driverId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

                dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
                dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
                dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
            }

            return Ok(dtoList);
        }

        [HttpGet("Store/{storeId:guid}")]
        public async Task<IActionResult> GetAllDeliveryTicketsByStoreId(Guid storeId)
        {
            var result = await _repository
                .GetDeliveryTicketsByConditionAsync(x => x.StoreId == storeId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

                dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
                dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
                dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
            }

            return Ok(dtoList);
        }

        [HttpGet("ServiceProvider/{serviceProviderId:guid}")]
        public async Task<IActionResult> GetAllDeliveryTicketsByServiceProviderId(Guid serviceProviderId)
        {
            var result = await _repository
                .GetDeliveryTicketsByConditionAsync(x => x.ServiceProviderId == serviceProviderId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

                dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
                dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
                dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
            }

            return Ok(dtoList);
        }

        [HttpGet("Summary/{id:guid}")]
        public async Task<IActionResult> GetDeliveryTicketSummaryById(Guid id)
        {
            var result = await _repository.GetDeliveryTicketSummaryByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            return Ok(result.DeliveryTicketSummary);
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryTicketDto>> CreateDeliveryTicket([FromBody] DeliveryTicketCreationDto deliveryTicketCreation)
        {
            var deliveryTicket = _mapper.Map<DeliveryTicketModel>(deliveryTicketCreation);
            deliveryTicket.CreatedAt = DateTime.UtcNow;

            var deliveryTicketNumberResult = await _repository.GetDeliveryTicketNumberAsync(deliveryTicket.ServiceProviderId);
            deliveryTicket.DeliveryTicketNumber = deliveryTicketNumberResult.DeliveryTicketNumber;

            var createResult = await _repository.CreateDeliveryTicketAsync(deliveryTicket);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
            }

            var dto = _mapper.Map<DeliveryTicketDto>(deliveryTicket);
            return CreatedAtRoute(nameof(GetDeliveryTicket), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDeliveryTicket(Guid id, DeliveryTicketUpdateDto companyUpdate)
        {
            var result = await _repository.GetDeliveryTicketByIdAsync(id);

            if (!result.IsSuccess || result.DeliveryTicket is null) return NotFound(result.ErrorMessage);
            _mapper.Map(companyUpdate, result.DeliveryTicket);
            result.DeliveryTicket.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateDeliveryTicketAsync(result.DeliveryTicket);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryTicket(Guid id)
        {
            var result = await _repository.DeleteDeliveryTicketAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }

    }
}

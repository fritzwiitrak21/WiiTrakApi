using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/deliverytickets")]
    [ApiController]
    public class DeliveryTicketsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryTicketRepository _repository;
        private readonly IDriverRepository _driverRepository;
        private readonly IStoreRepository _storeRepository;

        public DeliveryTicketsController(IMapper mapper, IDeliveryTicketRepository repository, IDriverRepository driverRepository, IStoreRepository storeRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _driverRepository = driverRepository;
            _storeRepository = storeRepository;
        }
        

        [HttpGet("{id:guid}", Name = "GetDeliveryTicket")]
        public async Task<IActionResult> GetDeliveryTicket(Guid id)
        {
            var result = await _repository.GetDeliveryTicketByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = _mapper.Map<DeliveryTicketDto>(result.DeliveryTicket);

            // get store name and number
            var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
            var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

            dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
            dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
            dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
            dto.TimezoneName = storeResult.Store != null ? $"{ storeResult.Store.TimezoneName }" : "";
            var TimeDiff = storeResult.Store != null ? $"{ storeResult.Store.TimezoneDiff }" : "";
            dto.TimezoneDiff = TimeDiff;
            if (TimeDiff != "")
            {
                dto.TimezoneDateTime = dto.DeliveredAt.AddSeconds(Convert.ToDouble(TimeDiff));
            }
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllDeliveryTickets()
        {
            var result = await _repository.GetAllDeliveryTicketsAsync();

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
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
            dtoList = dtoList.OrderByDescending(x => x.DeliveryTicketNumber).ToList();
            return Ok(dtoList);
        }

        [HttpGet("Driver/{driverId:guid}")]
        public async Task<IActionResult> GetAllDeliveryTicketsByDriverId(Guid driverId)
        {
            var result = await _repository
                .GetDeliveryTicketsByConditionAsync(x => x.DriverId == driverId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

                dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
                dto.DriverNumber = driverResult.Driver != null ? driverResult.Driver.DriverNumber : 0;
                dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
                dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
                dto.StreetAddress1 = storeResult.Store != null ? $"{ storeResult.Store.StreetAddress1 }" : "";
                dto.StreetAddress2 = storeResult.Store != null ? $"{ storeResult.Store.StreetAddress2 }" : "";
                dto.City = storeResult.Store != null ? $"{ storeResult.Store.City}" : "";
                dto.State = storeResult.Store != null ? $"{ storeResult.Store.State }" : "";
                dto.PostalCode = storeResult.Store != null ? $"{ storeResult.Store.PostalCode}" : "";
                dto.TimezoneName = storeResult.Store != null ? $"{ storeResult.Store.TimezoneName }" : "";
                var TimeDiff = storeResult.Store != null ? $"{ storeResult.Store.TimezoneDiff }" : "";
                dto.TimezoneDiff = TimeDiff;
                if (TimeDiff != "")
                {
                    dto.TimezoneDateTime = dto.DeliveredAt.AddSeconds(Convert.ToDouble(TimeDiff));
                }
            }
            dtoList = dtoList.OrderByDescending(x => x.DeliveryTicketNumber).ToList();
            return Ok(dtoList);
        }

        [HttpGet("Store/{storeId:guid}")]
        public async Task<IActionResult> GetAllDeliveryTicketsByStoreId(Guid storeId)
        {
            var result = await _repository
                .GetDeliveryTicketsByConditionAsync(x => x.StoreId == storeId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

                dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
                dto.DriverNumber = driverResult.Driver != null ? driverResult.Driver.DriverNumber : 0;
                dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
                dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
                dto.StreetAddress1 = storeResult.Store != null ? $"{ storeResult.Store.StreetAddress1 }" : "";
                dto.StreetAddress2 = storeResult.Store != null ? $"{ storeResult.Store.StreetAddress2 }" : "";
                dto.City = storeResult.Store != null ? $"{ storeResult.Store.City}" : "";
                dto.State = storeResult.Store != null ? $"{ storeResult.Store.State }" : "";
                dto.PostalCode = storeResult.Store != null ? $"{ storeResult.Store.PostalCode}" : "";
                dto.TimezoneName = storeResult.Store != null ? $"{ storeResult.Store.TimezoneName }" : "";
                var TimeDiff = storeResult.Store != null ? $"{ storeResult.Store.TimezoneDiff }" : "";
                dto.TimezoneDiff = TimeDiff;
                if (TimeDiff != "")
                {
                    dto.TimezoneDateTime = dto.DeliveredAt.AddSeconds(Convert.ToDouble(TimeDiff));
                }
            }
            dtoList = dtoList.OrderByDescending(x => x.DeliveryTicketNumber).ToList();
            return Ok(dtoList);
        }

        [HttpGet("ServiceProvider/{serviceProviderId:guid}")]
        public async Task<IActionResult> GetAllDeliveryTicketsByServiceProviderId(Guid serviceProviderId)
        {
            var result = await _repository
                .GetDeliveryTicketsByConditionAsync(x => x.ServiceProviderId == serviceProviderId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
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
            dtoList = dtoList.OrderByDescending(x => x.DeliveryTicketNumber).ToList();
            return Ok(dtoList);
        }
               
        [HttpGet("DeliveryTickets/{Id:guid}/{Role:int}")]
        public async Task<IActionResult> GetDeliveryTicketsByPrimaryId(Guid Id, int Role)
        {
            var result = await _repository
                .GetDeliveryTicketsByPrimaryIdAsync(Id, (Role)Role);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);
            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await _storeRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await _driverRepository.GetDriverByIdAsync(dto.DriverId);

                dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
                dto.DriverNumber = driverResult.Driver != null ?  driverResult.Driver.DriverNumber  : 0;
                dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
                dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
                dto.StreetAddress1 = storeResult.Store != null ? $"{ storeResult.Store.StreetAddress1 }" : "";
                dto.StreetAddress2 = storeResult.Store != null ? $"{ storeResult.Store.StreetAddress2 }" : "";
                dto.City = storeResult.Store != null ? $"{ storeResult.Store.City}" : "";
                dto.State = storeResult.Store != null ? $"{ storeResult.Store.State }" : "";
                dto.PostalCode = storeResult.Store != null ? $"{ storeResult.Store.PostalCode}" : "";
                dto.TimezoneName = storeResult.Store != null ? $"{ storeResult.Store.TimezoneName }" : "";
                var TimeDiff = storeResult.Store != null ? $"{ storeResult.Store.TimezoneDiff }" : "";
                dto.TimezoneDiff = TimeDiff;
                if (TimeDiff != "")
                {
                    dto.TimezoneDateTime = dto.DeliveredAt.AddSeconds(Convert.ToDouble(TimeDiff));
                }
            }
            dtoList= dtoList.OrderByDescending(x => x.DeliveryTicketNumber).ToList();
            return Ok(dtoList);
        }
             
        [HttpGet("Summary/{id:guid}")]
        public async Task<IActionResult> GetDeliveryTicketSummaryById(Guid id)
        {
            var result = await _repository.GetDeliveryTicketSummaryByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.DeliveryTicketSummary);
        }
        [HttpGet("DeliveryTickets/{Id:guid}/{Role:int}/{RecordCount:int}")]
        public async Task<IActionResult> GetDeliveryTicketsById(Guid Id, int Role,int RecordCount)
        {
            DateTime ToDate, FromDate;
            if (RecordCount == 0)
            {
                ToDate = DateTime.UtcNow;
                FromDate =Convert.ToDateTime("0001-01-01");
            }
            else
            {
                 ToDate = DateTime.UtcNow;
                 FromDate = ToDate.AddDays(-Convert.ToDouble(RecordCount));
            }
            var result = await _repository.GetDeliveryTicketsById(Id,(Role)Role,FromDate.ToString(),ToDate.ToString());
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.DeliveryTickets);
        }
       // [HttpPost("GetDeliveryTicketsByIdTest")]
        //public async Task<ActionResult<DeliveryTicketDto>> GetDeliveryTicketsByIdTest([FromBody] DeliveryTicketInputDto inputDto)
        //{
        //    var result = await _repository.GetDeliveryTicketsById(inputDto.Id,(Role)inputDto.RoleId,Convert.ToDateTime(inputDto.FromDate),Convert.ToDateTime(inputDto.ToDate));
        //    if (!result.IsSuccess)
        //    {
        //        return NotFound(result.ErrorMessage);
        //    }
        //    return Ok(result.DeliveryTickets); 
        //}
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
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<DeliveryTicketDto>(deliveryTicket);
            return CreatedAtRoute(nameof(GetDeliveryTicket), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDeliveryTicket(Guid id, DeliveryTicketUpdateDto companyUpdate)
        {
            var result = await _repository.GetDeliveryTicketByIdAsync(id);
            var Delivereddate = result.DeliveryTicket.DeliveredAt;
            if (!result.IsSuccess || result.DeliveryTicket is null)
            {
                return NotFound(result.ErrorMessage);
            }
            _mapper.Map(companyUpdate, result.DeliveryTicket);
            result.DeliveryTicket.UpdatedAt = DateTime.UtcNow;
            result.DeliveryTicket.DeliveredAt = Delivereddate;

            var updateResult = await _repository.UpdateDeliveryTicketAsync(result.DeliveryTicket);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryTicket(Guid id)
        {
            var result = await _repository.DeleteDeliveryTicketAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

    }
}

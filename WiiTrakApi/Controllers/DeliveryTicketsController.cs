/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
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
        private readonly IMapper Mapper;
        private readonly IDeliveryTicketRepository Repository;
        private readonly IDriverRepository DriverRepository;
        private readonly IStoreRepository StoreRepository;

        public DeliveryTicketsController(IMapper mapper, IDeliveryTicketRepository repository, IDriverRepository driverrepository, IStoreRepository storerepository)
        {
            Mapper = mapper;
            Repository = repository;
            DriverRepository = driverrepository;
            StoreRepository = storerepository;
        }


        [HttpGet("{id:guid}", Name = "GetDeliveryTicket")]
        public async Task<IActionResult> GetDeliveryTicket(Guid id)
        {
            var result = await Repository.GetDeliveryTicketByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<DeliveryTicketDto>(result.DeliveryTicket);

            // get store name and number
            var storeResult = await StoreRepository.GetStoreByIdAsync(dto.StoreId);
            var driverResult = await DriverRepository.GetDriverByIdAsync(dto.DriverId);

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
            var result = await Repository.GetAllDeliveryTicketsAsync();

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await StoreRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await DriverRepository.GetDriverByIdAsync(dto.DriverId);

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
            var result = await Repository
                .GetDeliveryTicketsByConditionAsync(x => x.DriverId == driverId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await StoreRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await DriverRepository.GetDriverByIdAsync(dto.DriverId);

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
            var result = await Repository
                .GetDeliveryTicketsByConditionAsync(x => x.StoreId == storeId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await StoreRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await DriverRepository.GetDriverByIdAsync(dto.DriverId);

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
            var result = await Repository
                .GetDeliveryTicketsByConditionAsync(x => x.ServiceProviderId == serviceProviderId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<DeliveryTicketDto>>(result.DeliveryTickets);

            // get store name and number
            foreach (var dto in dtoList)
            {
                var storeResult = await StoreRepository.GetStoreByIdAsync(dto.StoreId);
                var driverResult = await DriverRepository.GetDriverByIdAsync(dto.DriverId);

                dto.DriverName = driverResult.Driver != null ? $"{ driverResult.Driver.FirstName } { driverResult.Driver.LastName }" : "";
                dto.StoreName = storeResult.Store != null ? $"{ storeResult.Store.StoreName }" : "";
                dto.StoreNumber = storeResult.Store != null ? $"{ storeResult.Store.StoreNumber }" : "";
            }
            dtoList = dtoList.OrderByDescending(x => x.DeliveryTicketNumber).ToList();
            return Ok(dtoList);
        }


        [HttpGet("Summary/{id:guid}")]
        public async Task<IActionResult> GetDeliveryTicketSummaryById(Guid id)
        {
            var result = await Repository.GetDeliveryTicketSummaryByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.DeliveryTicketSummary);
        }
        [HttpGet("DeliveryTickets/{Id:guid}/{Role:int}/{RecordCount:int}")]
        public async Task<IActionResult> GetDeliveryTicketsById(Guid Id, int Role, int RecordCount)
        {
            DateTime ToDate, FromDate;
            if (RecordCount == 0)
            {
                ToDate = DateTime.UtcNow;
                FromDate = Convert.ToDateTime("0001-01-01");
            }
            else
            {
                ToDate = DateTime.UtcNow;
                FromDate = ToDate.AddDays(-Convert.ToDouble(RecordCount));
            }
            var result = await Repository.GetDeliveryTicketsById(Id, (Role)Role, FromDate.ToString(), ToDate.ToString());
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.DeliveryTickets);
        }
        [HttpGet("ServiceBoard/{Id:guid}/{Role:int}")]
        public async Task<IActionResult> GetServiceBoardDetailsById(Guid Id, int Role)
        {
            var result = await Repository.GetServiceBoardDetailsByRoleId(Id, (Role)Role);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.ServiceBoard);
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
            var deliveryTicket = Mapper.Map<DeliveryTicketModel>(deliveryTicketCreation);
            deliveryTicket.CreatedAt = DateTime.UtcNow;

            var deliveryTicketNumberResult = await Repository.GetDeliveryTicketNumberAsync(deliveryTicket.ServiceProviderId);
            deliveryTicket.DeliveryTicketNumber = deliveryTicketNumberResult.DeliveryTicketNumber;

            var createResult = await Repository.CreateDeliveryTicketAsync(deliveryTicket);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = Mapper.Map<DeliveryTicketDto>(deliveryTicket);
            return CreatedAtRoute(nameof(GetDeliveryTicket), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDeliveryTicket(Guid id, DeliveryTicketUpdateDto companyUpdate)
        {
            var result = await Repository.GetDeliveryTicketByIdAsync(id);
            var Delivereddate = result.DeliveryTicket.DeliveredAt;
            if (!result.IsSuccess || result.DeliveryTicket is null)
            {
                return NotFound(result.ErrorMessage);
            }
            Mapper.Map(companyUpdate, result.DeliveryTicket);
            result.DeliveryTicket.UpdatedAt = DateTime.UtcNow;
            result.DeliveryTicket.DeliveredAt = Delivereddate;

            var updateResult = await Repository.UpdateDeliveryTicketAsync(result.DeliveryTicket);
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
            var result = await Repository.DeleteDeliveryTicketAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

    }
}

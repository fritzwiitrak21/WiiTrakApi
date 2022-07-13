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
    [Route("api/carts")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly ICartRepository Repository;
        private readonly ICartHistoryRepository CartHistoryRepository;
        private readonly IDevicesRepository DevicesRepository;
        private readonly IDeviceHistoryRepository DeviceHistoryRepository;
        private readonly ITrackingDeviceRepository TrackingDeviceRepository;
        private readonly ITrackingDeviceHistoryRepository TrackingDeviceHistoryRepository;
        public CartsController(IMapper mapper,
            ICartRepository repository, ICartHistoryRepository Carthistoryrepository, IDevicesRepository Devicesrepository, IDeviceHistoryRepository Devicehistoryrepository, ITrackingDeviceRepository Trackingdevicerepository, ITrackingDeviceHistoryRepository Trackingdevicehistoryrepository)
        {
            Mapper = mapper;
            Repository = repository;
            CartHistoryRepository = Carthistoryrepository;
            DevicesRepository = Devicesrepository;
            DeviceHistoryRepository = Devicehistoryrepository;
            TrackingDeviceRepository = Trackingdevicerepository;
            TrackingDeviceHistoryRepository = Trackingdevicehistoryrepository;
        }

        [HttpGet("{id:guid}", Name = "GetCart")]
        public async Task<IActionResult> GetCart(Guid id)
        {
            var result = await Repository.GetCartByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<CartDto>(result.Cart);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCarts()
        {
            var result = await Repository.GetAllCartsAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CartDto>>(result.Carts);
            return Ok(dtoList);
        }

        [HttpGet("DeliveryTicket/{deliveryTicketId:guid}")]
        public async Task<IActionResult> GetCartsByDeliveryTicketId(Guid deliveryTicketId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await Repository.GetCartsByDeliveryTicketIdAsync(deliveryTicketId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CartDto>>(result.Carts);

            return Ok(dtoList);
        }

        [HttpGet("Store/{storeId:guid}")]
        public async Task<IActionResult> GetCartsByStoreId(Guid storeId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await Repository.GetCartsByStoreIdAsync(storeId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CartDto>>(result.Carts);

            return Ok(dtoList);
        }

        [HttpGet("Driver/{driverId:guid}")]
        public async Task<IActionResult> GetCartsByDriverId(Guid driverId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await Repository.GetCartsByDriverIdAsync(driverId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CartDto>>(result.Carts);
            return Ok(dtoList);
        }

        [HttpGet("Corporate/{corporateId:guid}")]
        public async Task<IActionResult> GetCartsByCorporateId(Guid corporateId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await Repository.GetCartsByCorporateIdAsync(corporateId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CartDto>>(result.Carts);

            return Ok(dtoList);
        }

        [HttpGet("Company/{companyId:guid}")]
        public async Task<IActionResult> GetCartsByCompanyId(Guid companyId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await Repository.GetCartsByCompanyIdAsync(companyId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CartDto>>(result.Carts);

            return Ok(dtoList);
        }

        [HttpGet("Technician/{technicianId:guid}")]
        public async Task<IActionResult> GetCartsByTechnicianId(Guid technicianId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await Repository.GetCartsByTechnicianIdAsync(technicianId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = Mapper.Map<List<CartDto>>(result.Carts);

            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateCart([FromBody] CartCreationDto cartCreation)
        {
            var cart = Mapper.Map<CartModel>(cartCreation);
            cart.CreatedAt = DateTime.UtcNow;

            var createResult = await Repository.CreateCartAsync(cart);
            if (!createResult.IsSuccess)
            {
                var CurrentDevice = await DevicesRepository.GetDeviceByIdAsync(cart.DeviceId);
                var Trackingdevice = await TrackingDeviceRepository.GetTrackingDevicebyIMEIAsync(CurrentDevice.DeviceList.IMEI);
                CurrentDevice.DeviceList.IsMapped = true;
                CurrentDevice.DeviceList.UpdatedAt = DateTime.UtcNow;
                if (Trackingdevice.TrackingDevice != null)
                {
                    Trackingdevice.TrackingDevice.CartId = cart.Id;
                    Trackingdevice.TrackingDevice.UpdatedAt = DateTime.UtcNow;
                    var TrackingDeviceHistory = new TrackingDeviceHistoryModel
                    {
                        TrackingDeviceId = Trackingdevice.TrackingDevice.Id,
                        Longitude = Trackingdevice.TrackingDevice.Longitude,
                        Latitude = Trackingdevice.TrackingDevice.Latitude,
                        SIMCardId = new Guid(Trackingdevice.TrackingDevice.SIMCardId),
                        CartId = Trackingdevice.TrackingDevice.CartId,
                        CreatedAt = DateTime.UtcNow
                    };
                    await TrackingDeviceHistoryRepository.CreateTrackingDeviceHistoryAsync(TrackingDeviceHistory);
                    await TrackingDeviceRepository.UpdateTrackingDeviceAsync(Trackingdevice.TrackingDevice);
                }
                var DeviceHistory = new DeviceHistoryModel
                {
                    DeviceId = cart.DeviceId,
                    CartId = cart.Id,
                    MappedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    TechnicianId = cartCreation.CreatedBy
                };
                await DeviceHistoryRepository.CreateDeviceHistoryAsync(DeviceHistory);
                await DevicesRepository.UpdateDeviceAsync(CurrentDevice.DeviceList);
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = Mapper.Map<CartDto>(cart);
            return CreatedAtRoute(nameof(GetCart), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCart(Guid id, CartUpdateDto cartUpdate)
        {
            try
            {
                var result = await Repository.GetCartByIdAsync(id);
                var cart = result.Cart;
                var CurrentDevice = await DevicesRepository.GetDeviceByIdAsync(cartUpdate.DeviceId);
                var PreviousDevice = await DevicesRepository.GetDeviceByIdAsync(result.Cart.DeviceId);
                var PreviousDeviceHistory = new DeviceHistoryModel();
                var CurrentDeviceHistory = new DeviceHistoryModel();
                if (cartUpdate.CartHistory.DriverId != Guid.Empty)
                {
                    //update cart history
                    var cartHistory = Mapper.Map<CartHistoryModel>(cartUpdate.CartHistory);
                    cartHistory.CreatedAt = DateTime.UtcNow;
                    await CartHistoryRepository.UpdateCartHistoryAsync(cartHistory);
                }

                if (!result.IsSuccess || cart is null)
                {
                    return NotFound(result.ErrorMessage);
                }

                cart.BarCode = cartUpdate.BarCode;
                cart.Condition = cartUpdate.Condition;
                cart.Status = cartUpdate.Status;
                cart.DateManufactured = cartUpdate.DateManufactured;
                cart.IsProvisioned = cartUpdate.IsProvisioned;
                cart.ManufacturerName = cartUpdate.ManufacturerName;
                cart.CartNumber = cartUpdate.CartNumber;
                cart.OrderedFrom = cartUpdate.OrderedFrom;
                cart.PicUrl = cartUpdate.PicUrl;
                cart.StoreId = cartUpdate.StoreId;
                cart.UpdatedAt = DateTime.UtcNow;
                cart.IssueType = cartUpdate.IssueType;
                cart.IssueDescription = cartUpdate.IssueDescription;
                cart.DeviceId = cartUpdate.DeviceId;
                var updateResult = await Repository.UpdateCartAsync(cart);
                if (updateResult.IsSuccess)
                {
                    try
                    {
                        var Trackingdevice = await TrackingDeviceRepository.GetTrackingDeviceByIdAsync(cartUpdate.DeviceId);
                        if (CurrentDevice.DeviceList != null)
                        {

                            Trackingdevice = await TrackingDeviceRepository.GetTrackingDevicebyIMEIAsync(CurrentDevice.DeviceList.IMEI);
                            Trackingdevice.TrackingDevice.CartId = cart.Id;
                        }
                        else if (PreviousDevice.DeviceList != null)
                        {
                            Trackingdevice = await TrackingDeviceRepository.GetTrackingDevicebyIMEIAsync(PreviousDevice.DeviceList.IMEI);
                            Trackingdevice.TrackingDevice.CartId = Guid.Empty;
                        }
                        if (Trackingdevice.TrackingDevice != null)
                        {
                            Trackingdevice.TrackingDevice.UpdatedAt = DateTime.UtcNow;
                            var TrackingDeviceHistory = new TrackingDeviceHistoryModel
                            {
                                TrackingDeviceId = Trackingdevice.TrackingDevice.Id,
                                Longitude = Trackingdevice.TrackingDevice.Longitude,
                                Latitude = Trackingdevice.TrackingDevice.Latitude,
                                SIMCardId = new Guid(Trackingdevice.TrackingDevice.SIMCardId),
                                CartId = Trackingdevice.TrackingDevice.CartId,
                                CreatedAt = DateTime.UtcNow
                            };
                            await TrackingDeviceHistoryRepository.CreateTrackingDeviceHistoryAsync(TrackingDeviceHistory);
                            await TrackingDeviceRepository.UpdateTrackingDeviceAsync(Trackingdevice.TrackingDevice);
                        }
                    }
                    catch (Exception ex)
                    {

                    }


                    if (PreviousDevice.DeviceList != null)
                    {
                        PreviousDeviceHistory.DeviceId = PreviousDevice.DeviceList.Id;
                        PreviousDeviceHistory.CartId = cart.Id;
                        PreviousDeviceHistory.RemovedAt = DateTime.UtcNow;
                        PreviousDeviceHistory.CreatedAt = DateTime.UtcNow;
                        PreviousDeviceHistory.TechnicianId = cartUpdate.CreatedBy;
                        PreviousDeviceHistory.IsActive = true;
                    }
                    if (CurrentDevice.DeviceList != null)
                    {
                        CurrentDeviceHistory.DeviceId = cart.DeviceId;
                        CurrentDeviceHistory.CartId = cart.Id;
                        CurrentDeviceHistory.MappedAt = DateTime.UtcNow;
                        CurrentDeviceHistory.CreatedAt = DateTime.UtcNow;
                        CurrentDeviceHistory.TechnicianId = cartUpdate.CreatedBy;
                        CurrentDeviceHistory.IsActive = true;

                    }
                    if (cartUpdate.DeviceId != Guid.Empty)
                    {

                        if (PreviousDevice.DeviceList != null && CurrentDevice.DeviceList != null && CurrentDevice.DeviceList.Id != PreviousDevice.DeviceList.Id)
                        {
                            PreviousDevice.DeviceList.IsMapped = false;
                            PreviousDevice.DeviceList.UpdatedAt = DateTime.UtcNow;
                            CurrentDevice.DeviceList.IsMapped = true;
                            CurrentDevice.DeviceList.UpdatedAt = DateTime.UtcNow;
                            await DeviceHistoryRepository.CreateDeviceHistoryAsync(PreviousDeviceHistory);
                            await DeviceHistoryRepository.CreateDeviceHistoryAsync(CurrentDeviceHistory);
                            await DevicesRepository.UpdateDeviceAsync(PreviousDevice.DeviceList);
                            await DevicesRepository.UpdateDeviceAsync(CurrentDevice.DeviceList);
                        }
                        else if (PreviousDevice.DeviceList != null)
                        {
                            PreviousDevice.DeviceList.IsMapped = false;
                            PreviousDevice.DeviceList.UpdatedAt = DateTime.UtcNow;
                            await DeviceHistoryRepository.CreateDeviceHistoryAsync(PreviousDeviceHistory);
                            await DevicesRepository.UpdateDeviceAsync(PreviousDevice.DeviceList);
                        }
                        else if (CurrentDevice.DeviceList != null)
                        {
                            CurrentDevice.DeviceList.IsMapped = true;
                            CurrentDevice.DeviceList.UpdatedAt = DateTime.UtcNow;
                            await DeviceHistoryRepository.CreateDeviceHistoryAsync(CurrentDeviceHistory);
                            await DevicesRepository.UpdateDeviceAsync(CurrentDevice.DeviceList);
                        }
                    }
                    else
                    {
                        PreviousDevice.DeviceList.IsMapped = false;
                        PreviousDevice.DeviceList.UpdatedAt = DateTime.UtcNow;
                        await DevicesRepository.UpdateDeviceAsync(PreviousDevice.DeviceList);
                        await DeviceHistoryRepository.CreateDeviceHistoryAsync(PreviousDeviceHistory);
                        
                    }

                    return NoContent();
                }
                ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            var result = await Repository.DeleteCartAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

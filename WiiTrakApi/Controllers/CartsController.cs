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

        public CartsController(IMapper mapper,
            ICartRepository repository, ICartHistoryRepository Carthistoryrepository)
        {
            Mapper = mapper;
            Repository = repository;
            CartHistoryRepository = Carthistoryrepository;
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

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateCart([FromBody] CartCreationDto cartCreation)
         {
            var cart = Mapper.Map<CartModel>(cartCreation);
            cart.CreatedAt = DateTime.UtcNow;

            var createResult = await Repository.CreateCartAsync(cart);
            if (!createResult.IsSuccess)
            {
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
                var updateResult = await Repository.UpdateCartAsync(cart);
                if (updateResult.IsSuccess)
                {
                    return NoContent();
                }
                ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }
            catch(Exception ex)
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

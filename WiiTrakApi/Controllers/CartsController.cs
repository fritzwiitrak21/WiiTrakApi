/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _repository;
        private readonly ICartHistoryRepository _cartHistoryRepository;

        string[] _cartImgUrls = new[]
        {
            "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/103-172-red-45-degree-view.jpg",
            "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/1725580.jpg",
            "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/46051_1000.jpg",
            "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/H-4568.jpg",
        };


        static Random _randomizer = new Random();

        public CartsController(IMapper mapper,
            ICartRepository repository, ICartHistoryRepository cartHistoryRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _cartHistoryRepository = cartHistoryRepository;
        }

        [HttpGet("{id:guid}", Name = "GetCart")]
        public async Task<IActionResult> GetCart(Guid id)
        {
            var result = await _repository.GetCartByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = _mapper.Map<CartDto>(result.Cart);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCarts()
        {
            var result = await _repository.GetAllCartsAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<CartDto>>(result.Carts);
            return Ok(dtoList);
        }

        [HttpGet("DeliveryTicket/{deliveryTicketId:guid}")]
        public async Task<IActionResult> GetCartsByDeliveryTicketId(Guid deliveryTicketId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await _repository.GetCartsByDeliveryTicketIdAsync(deliveryTicketId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            var dtoList = _mapper.Map<List<CartDto>>(result.Carts);


            foreach (var cart in dtoList)
            {
                cart.PicUrl = _cartImgUrls[_randomizer.Next(_cartImgUrls.Length)];
            }

            return Ok(dtoList);
        }

        [HttpGet("Store/{storeId:guid}")]
        public async Task<IActionResult> GetCartsByStoreId(Guid storeId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await _repository.GetCartsByStoreIdAsync(storeId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            var dtoList = _mapper.Map<List<CartDto>>(result.Carts);


            foreach (var cart in dtoList)
            {
                cart.PicUrl = _cartImgUrls[_randomizer.Next(_cartImgUrls.Length)];
            }

            return Ok(dtoList);
        }

        [HttpGet("Driver/{driverId:guid}")]
        public async Task<IActionResult> GetCartsByDriverId(Guid driverId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await _repository.GetCartsByDriverIdAsync(driverId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            var dtoList = _mapper.Map<List<CartDto>>(result.Carts);

            return Ok(dtoList);
        }

        [HttpGet("Corporate/{corporateId:guid}")]
        public async Task<IActionResult> GetCartsByCorporateId(Guid corporateId)
        {

            // Returns carts with outside geofence and picked up statuses
            var result = await _repository.GetCartsByCorporateIdAsync(corporateId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            var dtoList = _mapper.Map<List<CartDto>>(result.Carts);


            foreach (var cart in dtoList)
            {
                cart.PicUrl = _cartImgUrls[_randomizer.Next(_cartImgUrls.Length)];
            }

            return Ok(dtoList);
        }

        [HttpGet("Company/{companyId:guid}")]
        public async Task<IActionResult> GetCartsByCompanyId(Guid companyId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await _repository.GetCartsByCompanyIdAsync(companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            var dtoList = _mapper.Map<List<CartDto>>(result.Carts);


            foreach (var cart in dtoList)
            {
                cart.PicUrl = _cartImgUrls[_randomizer.Next(_cartImgUrls.Length)];
            }

            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateCart([FromBody] CartCreationDto cartCreation)
         {
            var cart = _mapper.Map<CartModel>(cartCreation);
            cart.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateCartAsync(cart);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<CartDto>(cart);
            return CreatedAtRoute(nameof(GetCart), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCart(Guid id, CartUpdateDto cartUpdate)
        {
            try
            {
                if (cartUpdate.CartHistory.DriverId != Guid.Empty)
                {
                    //update cart history
                    var cartHistory = _mapper.Map<CartHistoryModel>(cartUpdate.CartHistory);
                    cartHistory.CreatedAt = DateTime.Now;
                    await _cartHistoryRepository.CreateCartHistoryAsync(cartHistory);
                }
                var result = await _repository.GetCartByIdAsync(id);
                var cart = result.Cart;

                if (!result.IsSuccess || cart is null) return NotFound(result.ErrorMessage);


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

                var updateResult = await _repository.UpdateCartAsync(cart);
                if (updateResult.IsSuccess) return NoContent();
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
            var result = await _repository.DeleteCartAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

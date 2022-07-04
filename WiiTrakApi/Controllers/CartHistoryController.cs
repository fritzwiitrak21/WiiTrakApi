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
    [Route("api/carthistory")]
    [ApiController]
    public class CartHistoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICartHistoryRepository _repository;
        public CartHistoryController(IMapper mapper, ICartHistoryRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetCartHistory")]
        public async Task<IActionResult> GetCartHistory(Guid id)
        {
            var result = await _repository.GetCartHistoryByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = _mapper.Map<CartHistoryDto>(result.CartHistory);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllCartHistory()
        {
            var result = await _repository.GetAllCartHistoryAsync();

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<CartHistoryDto>>(result.CartHistory);
            return Ok(dtoList);
        }

        [HttpGet("Cart/{cartId:guid}")]
        public async Task<IActionResult> GetCartHistoryByCartId(Guid cartId)
        {
            var result = await _repository.GetCartHistoryByCartIdAsync(cartId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            var dtoList = _mapper.Map<List<CartHistoryDto>>(result.CartHistory);

            return Ok(dtoList);
        }

        [HttpGet("Store/{storeId:guid}")]
        public async Task<IActionResult> GetCartHistoryByStoreId(Guid storeId)
        {
            var result = await _repository.GetCartHistoryByStoreIdAsync(storeId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            var dtoList = _mapper.Map<List<CartHistoryDto>>(result.CartHistory);

            return Ok(dtoList);
        }

        [HttpGet("ServiceProvider/{serviceProviderId:guid}")]
        public async Task<IActionResult> GetCartHistoryByServiceProviderId(Guid serviceProviderId)
        {
            var result = await _repository.GetCartHistoryByServiceProviderIdAsync(serviceProviderId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            var dtoList = _mapper.Map<List<CartHistoryDto>>(result.CartHistory);

            return Ok(dtoList);
        }
        [HttpGet("CartHistory/{deliveryTicketId:guid}")]
        public async Task<IActionResult> GetCartHistoryByDeliveryTicketId(Guid deliveryTicketId)
        {
            // Returns carts with outside geofence and picked up statuses
            var result = await _repository.GetCartHistoryByDeliveryTicketIdAsync(deliveryTicketId);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtoList = _mapper.Map<List<CartDto>>(result.Carts);
            return Ok(dtoList);
        }
        [HttpPost]
        public async Task<ActionResult<CartHistoryDto>> CreateCartHistory([FromBody] CartHistoryCreationDto cartHistoryCreation)
        {
            var cartHistory = _mapper.Map<CartHistoryModel>(cartHistoryCreation);
            cartHistory.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateCartHistoryAsync(cartHistory);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<CartHistoryDto>(cartHistory);
            return CreatedAtRoute(nameof(GetCartHistory), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCartHistory(Guid id, CartHistoryUpdateDto cartHistoryUpdate)
        {
            var result = await _repository.GetCartHistoryByIdAsync(id);

            if (!result.IsSuccess || result.CartHistory is null)
            {
                return NotFound(result.ErrorMessage);
            }
            _mapper.Map(cartHistoryUpdate, result.CartHistory);
            result.CartHistory.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateCartHistoryAsync(result.CartHistory);
            if (updateResult.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartHistory(Guid id)
        {
            var result = await _repository.DeleteCartHistoryAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }

    }
}

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
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly IMessagesRepository Repository;

        public MessagesController(IMapper mapper, IMessagesRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            var result = await Repository.GetMessageAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<CartDto>(result.Message);
            return Ok(dto);
        }
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var result = await Repository.GetAllMessagesAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dto = Mapper.Map<List<MessagesDto>>(result.Messages);
            return Ok(dto);
        }
        
        [HttpGet("{Id:guid}/{RoleId:int}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessageById(Guid Id, int RoleId)
        {
            try
            {
                var result = await Repository.GetMessagesBIdAsync(Id, RoleId);
                if (!result.IsSuccess)
                {
                    return NotFound(result.ErrorMessage);
                }
                var dtolist = Mapper.Map<List<MessagesDto>>(result.Messages);
                return Ok(dtolist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<MessagesDto>> AddNewMessage([FromBody] MessagesDto Message)
        {
            var NewMessage = Mapper.Map<MessagesModel>(Message);
            NewMessage.CreatedAt = DateTime.UtcNow;

            var createResult = await Repository.AddNewMessageAsync(NewMessage);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = Mapper.Map<MessagesDto>(Message);
            return CreatedAtRoute(nameof(GetMessage), new { id = dto.Id }, dto);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMessageDeliveredTime(MessagesDto Message)
        {

            var result = await Repository.UpdateMessageDeliveredTimeAsync(Message.Id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMessageDeliveredTime(Guid Id, MessagesDto Message)
        {

            var result = await Repository.UpdateMessageActionAsync(Id, Message.ActionTaken);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

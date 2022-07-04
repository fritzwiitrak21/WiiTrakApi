/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMapper Mapper;
        private readonly INotificationRepository Repository;

        public NotificationController(IMapper mapper, INotificationRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllNotification()
        {
            var result = await Repository.GetAllNotificationsAsync();
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }
            var dtolist = Mapper.Map<List<NotificationDto>>(result.Notification);
            return Ok(dtolist);
        }

        [HttpGet("{Id:guid}", Name = "GetNotification")]
        public async Task<IActionResult> GetNotification(Guid Id)
        {
            try
            {
                var result = await Repository.GetNotificationAsync(Id);
                if (!result.IsSuccess)
                {
                    return NotFound(result.ErrorMessage);
                }
                var dtolist = Mapper.Map<List<NotificationDto>>(result.Notification);
                return Ok(dtolist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotifiedTime(NotificationDto dto)
        {
            var result = await Repository.UpdateNotifiedTimeAsync(dto.Id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

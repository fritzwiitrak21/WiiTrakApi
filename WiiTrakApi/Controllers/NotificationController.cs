﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IMapper _mapper;
        private readonly INotificationRepository _repository;

        public NotificationController(ILogger<NotificationController> logger, IMapper mapper, INotificationRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllNotification()
        {
            var result = await _repository.GetAllNotificationsAsync();
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtolist = _mapper.Map<List<NotificationDto>>(result.Notification);
            return Ok(dtolist);
        }

        [HttpGet("{id:guid}", Name = "GetNotification")]
        public async Task<IActionResult> GetNotification(Guid Id)
        {
            var result = await _repository.GetNotificationAsync(Id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtolist = _mapper.Map<List<NotificationDto>>(result.Notification);
            return Ok(dtolist);
        }
    }
}
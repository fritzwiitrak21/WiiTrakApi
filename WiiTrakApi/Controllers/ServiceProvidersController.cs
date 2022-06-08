/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/serviceproviders")]
    [ApiController]
    public class ServiceProvidersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceProviderRepository _repository;

        public ServiceProvidersController(IMapper mapper,
            IServiceProviderRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetServiceProvider")]
        public async Task<IActionResult> GetServiceProvider(Guid id)
        {
            var result = await _repository.GetServiceProviderByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<ServiceProviderDto>(result.ServiceProvider);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllServiceProviders()
        {
            var result = await _repository.GetAllServiceProvidersAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<ServiceProviderDto>>(result.ServiceProviders);
            return Ok(dtoList);
        }

        [HttpGet("{companyId:guid}")]
        public async Task<IActionResult> GetServiceProvidersByCustomerId(Guid companyId)
        {
            var result =
                await _repository.GetServiceProvidersByConditionAsync(x => x.CompanyId == companyId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<ServiceProviderDto>>(result.ServiceProviders);
            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceProviderDto>> CreateServiceProvider([FromBody] ServiceProviderCreationDto serviceProviderCreation)
        {
            var serviceProvider = _mapper.Map<ServiceProviderModel>(serviceProviderCreation);
            serviceProvider.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateServiceProviderAsync(serviceProvider);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<ServiceProviderDto>(serviceProvider);
            return CreatedAtRoute(nameof(GetServiceProvider), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateServiceProvider(Guid id, ServiceProviderUpdateDto serviceProviderUpdate)
        {
            var result = await _repository.GetServiceProviderByIdAsync(id);

            if (!result.IsSuccess || result.ServiceProvider is null) return NotFound(result.ErrorMessage);
            _mapper.Map(serviceProviderUpdate, result.ServiceProvider);
            result.ServiceProvider.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateServiceProviderAsync(result.ServiceProvider);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceProvider(Guid id)
        {
            var result = await _repository.DeleteServiceProviderAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/trackingdevice")]
    [ApiController]
    public class TrackingDeviceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITrackingDeviceRepository _repository;

        public TrackingDeviceController(IMapper mapper,
            ITrackingDeviceRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetTrackingDevice")]
        public async Task<IActionResult> GetTrackingDevice(Guid id)
        {
            var result = await _repository.GetTrackingDeviceByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<TrackingDeviceDto>(result.TrackingDevice);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllTrackingDevices()
        {
            var result = await _repository.GetAllTrackingDevicesAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<TrackingDeviceDto>>(result.TrackingDevices);
            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackingDeviceDto>> CreateTrackingDevice([FromBody] TrackingDeviceCreationDto trackingDeviceCreation)
        {
            var trackingDevice = _mapper.Map<TrackingDeviceModel>(trackingDeviceCreation);
            trackingDevice.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateTrackingDeviceAsync(trackingDevice);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", Cores.Core.SaveErrorMessage);
                return StatusCode(Cores.Numbers.FiveHundred, ModelState);
            }

            var dto = _mapper.Map<TrackingDeviceDto>(trackingDevice);
            return CreatedAtRoute(nameof(GetTrackingDevice), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTrackingDevice(Guid id, TrackingDeviceUpdateDto trackingDeviceUpdate)
        {
            var result = await _repository.GetTrackingDeviceByIdAsync(id);

            if (!result.IsSuccess || result.TrackingDevice is null) return NotFound(result.ErrorMessage);
            _mapper.Map(trackingDeviceUpdate, result.TrackingDevice);
            result.TrackingDevice.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateTrackingDeviceAsync(result.TrackingDevice);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
        [HttpPut]
        public async Task<IActionResult> GetCoordinatesOfDevices()
        {

            //var result = await _repository.GetTrackingDeviceByIdAsync(id);

            //if (!result.IsSuccess || result.TrackingDevice is null) return NotFound(result.ErrorMessage);
            //_mapper.Map(trackingDeviceUpdate, result.TrackingDevice);
            //result.TrackingDevice.UpdatedAt = DateTime.UtcNow;


            //var updateResult = await _repository.UpdateTrackingDeviceAsync(result.TrackingDevice);
            //if (updateResult.IsSuccess) return NoContent();

            //error
            ModelState.AddModelError("", Cores.Core.UpdateErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrackingDevice(Guid id)
        {
            var result = await _repository.DeleteTrackingDeviceAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", Cores.Core.DeleteErrorMessage);
            return StatusCode(Cores.Numbers.FiveHundred, ModelState);
        }
    }
}

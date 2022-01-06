using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly ILogger<AssetsController> _logger;
        private readonly IMapper _mapper;
        private readonly IAssetRepository _repository;


        static Random _randomizer = new Random();

        public AssetsController(ILogger<AssetsController> logger,
            IMapper mapper,
            IAssetRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("{id:guid}", Name = "GetAsset")]
        public async Task<IActionResult> GetAsset(Guid id)
        {
            var result = await _repository.GetAssetByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dto = _mapper.Map<AssetDto>(result.Asset);
            return Ok(dto);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllAssets()
        {
            var result = await _repository.GetAllAssetsAsync();

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<AssetDto>>(result.Assets);
            return Ok(dtoList);
        }

        [HttpGet("Store/{store:guid}")]
        [EnableQuery]
        public async Task<IActionResult> GetAssetsByStoreId(Guid storeId)
        {
            var result = await _repository
                .GetAssetsByConditionAsync(x => x.StoreId == storeId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            var dtoList = _mapper.Map<List<AssetDto>>(result.Assets);
            return Ok(dtoList);
        }

        [HttpGet("Driver/{driverId:guid}")]
        public async Task<IActionResult> GetAssetsByDriverId(Guid driverId)
        {
            string[] cartImgUrls = new[]
            {
                "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/103-172-red-45-degree-view.jpg",
                "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/1725580.jpg",
                "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/46051_1000.jpg",
                "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/H-4568.jpg",
            };



            // Returns assets with outside geofence and picked up statuses
            var result = await _repository.GetAssetsByDriverIdAsync(driverId);

            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            var dtoList = _mapper.Map<List<AssetDto>>(result.Assets);
            
           
            foreach (var cart in dtoList)
            {
                cart.PicUrl = cartImgUrls[_randomizer.Next(cartImgUrls.Length)];
            }

            return Ok(dtoList);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AssetDto>> CreateAsset([FromBody] AssetCreationDto assetCreation)
        {
            var asset = _mapper.Map<AssetModel>(assetCreation);
            asset.CreatedAt = DateTime.UtcNow;

            var createResult = await _repository.CreateAssetAsync(asset);
            if (!createResult.IsSuccess)
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record.");
                return StatusCode(500, ModelState);
            }

            var dto = _mapper.Map<AssetDto>(asset);
            return CreatedAtRoute(nameof(GetAsset), new { id = dto.Id }, dto);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsset(Guid id, AssetUpdateDto assetUpdate)
        {
            var result = await _repository.GetAssetByIdAsync(id);

            if (!result.IsSuccess || result.Asset is null) return NotFound(result.ErrorMessage);
            _mapper.Map(assetUpdate, result.Asset);
            result.Asset.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _repository.UpdateAssetAsync(result.Asset);
            if (updateResult.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when updating the record.");
            return StatusCode(500, ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            var result = await _repository.DeleteAssetAsync(id);
            if (result.IsSuccess) return NoContent();

            ModelState.AddModelError("", $"Something went wrong when deleting the record.");
            return StatusCode(500, ModelState);
        }
    }
}

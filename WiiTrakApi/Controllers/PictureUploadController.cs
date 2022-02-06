using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.Services.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class PictureUploadController : ControllerBase
    {
        private readonly ILogger<CartsController> _logger;
        private readonly IUploadService uploadService;
        private readonly string _imageBlobContainerName;

        public PictureUploadController(ILogger<CartsController> logger,
            IUploadService uploadService, IConfiguration configuration)
        {
            _logger = logger;
            this.uploadService = uploadService;
            _imageBlobContainerName = configuration["ImageBlobContainerName"];
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    // use guid for file name
                    var fileExtension = Path.GetExtension(fileName);
                    var blobFileName = $"{ Guid.NewGuid() }{ fileExtension.ToLower() }";

                    string fileURL = await uploadService.UploadAsync(file.OpenReadStream(), blobFileName, file.ContentType, _imageBlobContainerName);
                    return Ok(new { fileURL });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        
        [HttpPost("Signature"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadSignatureAsync([FromForm] IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    // use guid for file name
                    var fileExtension = Path.GetExtension(fileName);
                    var blobFileName = $"{ Guid.NewGuid() }{ fileExtension.ToLower() }";

                    string fileURL = await uploadService.UploadAsync(file.OpenReadStream(), blobFileName, file.ContentType, _imageBlobContainerName);
                    return Ok(new { fileURL });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}

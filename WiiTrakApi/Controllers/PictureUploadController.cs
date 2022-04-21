using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.Services.Contracts;
using WiiTrakApi.DTOs;

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

                    var Latitude = formCollection.FirstOrDefault(x => x.Key.Equals("Latitude")).Value.ToString();
                    var Longitude = formCollection.FirstOrDefault(x => x.Key.Equals("Longitude")).Value.ToString();
                    var Date = formCollection.FirstOrDefault(x => x.Key.Equals("Date")).Value.ToString();

                    var watermarkedStream = new MemoryStream();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);

                        // Add watermark
                        watermarkedStream = new MemoryStream();
                        using (var img = Image.FromStream(stream))
                        {
                            using (var graphic = Graphics.FromImage(img))
                            {
                                var font = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel);
                                var Coordfont = new Font("Arial", 15, FontStyle.Regular, GraphicsUnit.Pixel);
                                var color = Color.Black;// FromArgb(255, 255, 255, 255);
                                var backcolor = Color.FromArgb(100, 220, 220, 220);
                                var brush = new SolidBrush(color);
                                var width = img.Width;
                                var height = img.Height;
                                var datepoint = new Point(5, img.Height - 45);
                                var latpoint = new Point(5, img.Height - 22);
                                var longpoint = new Point(width / 2, img.Height - 22);

                                Rectangle rect = new Rectangle(0, height - 48, width, height - 48);

                                graphic.FillRectangle(new SolidBrush(backcolor), rect);
                                graphic.DrawString(Date, font, brush, datepoint);
                                graphic.DrawString("Coord: " + Latitude + "," + Longitude, Coordfont, brush, latpoint);
                                
                                img.Save(watermarkedStream, ImageFormat.Png);
                            }
                        }
                    }

                    //var response = await _imageStorageProvider.InsertAsync(name, watermarkedStream.ToArray());
                    string fileURL = await uploadService.UploadAsync(watermarkedStream, blobFileName, file.ContentType, _imageBlobContainerName, true);
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

        [HttpPost("Coord/{Latitude}/{Longitude}")]
        public async Task<IActionResult> UploadPictureWithCoordsAsync(string Latitude,string Longitude, [FromForm] IFormFile file)
        {
            try
            {
                
                //var formCollection = await Request.ReadFormAsync();
                //var file = formCollection.Files.First();
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    // use guid for file name
                    var fileExtension = Path.GetExtension(fileName);
                    var blobFileName = $"{ Guid.NewGuid() }{ fileExtension.ToLower() }";

                    var watermarkedStream = new MemoryStream();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);

                        // Add watermark
                        watermarkedStream = new MemoryStream();
                        using (var img = Image.FromStream(stream))
                        {
                            using (var graphic = Graphics.FromImage(img))
                            {
                                var font = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel);
                                var color = Color.Black;// FromArgb(255, 255, 255, 255);
                                var backcolor = Color.FromArgb(100, 220, 220, 220);
                                var brush = new SolidBrush(color);
                                var width = img.Width;
                                var height = img.Height;
                                var datepoint = new Point(5, img.Height - 45);
                                var latpoint = new Point(5, img.Height - 22);
                                var longpoint = new Point(width / 2, img.Height - 22);

                                Pen pen = new Pen(backcolor, 3);

                                Rectangle rect = new Rectangle(0, height - 48, width, height - 48);

                                // Draw rectangle to screen.
                                //graphic.DrawRectangle(pen, 45, width, width, height- 45);
                                graphic.FillRectangle(new SolidBrush(backcolor), rect);
                                graphic.DrawString(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), font, brush, datepoint);
                                graphic.DrawString("COORD : "+ Latitude+","+ Longitude, font, brush, latpoint);
                                //graphic.DrawString("Longitude : ", font, brush, longpoint);
                                img.Save(watermarkedStream, ImageFormat.Png);
                            }
                        }
                    }

                    //var response = await _imageStorageProvider.InsertAsync(name, watermarkedStream.ToArray());
                    string fileURL = await uploadService.UploadAsync(watermarkedStream, blobFileName, file.ContentType, _imageBlobContainerName, true);
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

        [HttpPost("Picture"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadPictureAsync([FromForm] IFormFile file)
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

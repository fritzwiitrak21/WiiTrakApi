using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.Services.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class PictureUploadController : ControllerBase
    {
        private readonly IUploadService uploadService;
        private readonly string ImageBlobContainerName;

        public PictureUploadController(IUploadService uploadService, IConfiguration configuration)
        {
            this.uploadService = uploadService;
            ImageBlobContainerName = configuration["ImageBlobContainerName"];
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

                    MemoryStream watermarkedStream;
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);

                        // Add watermark
                        watermarkedStream = new MemoryStream();
                        using (var img = Image.FromStream(stream))
                        {
                            using (var graphic = Graphics.FromImage(img))
                            {
                                var font = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                                var Coordfont = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                                var color = Color.Black;
                                var backcolor = Color.FromArgb(100, 220, 220, 220);
                                var brush = new SolidBrush(color);
                                var width = img.Width;
                                var height = img.Height;
                                var datepoint = new Point(5, img.Height - 32);
                                var latpoint = new Point(5, img.Height - 22);
                                var longpoint = new Point(5, img.Height - 12);

                                Rectangle rect = new Rectangle(0, height - 33, width, height - 33);

                                graphic.FillRectangle(new SolidBrush(backcolor), rect);
                                graphic.DrawString(Date, font, brush, datepoint);
                                graphic.DrawString("LAT: " + Latitude, Coordfont, brush, latpoint);
                                graphic.DrawString("LONG: " + Longitude, Coordfont, brush, longpoint);

                                img.Save(watermarkedStream, ImageFormat.Png);
                            }
                        }
                    }

                    string fileURL = await uploadService.UploadAsync(watermarkedStream, blobFileName, file.ContentType, ImageBlobContainerName, true);
                    return Ok(new { fileURL });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Cores.Numbers.FiveHundred, $"Internal server error: {ex}");
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

                    string fileURL = await uploadService.UploadAsync(file.OpenReadStream(), blobFileName, file.ContentType, ImageBlobContainerName);
                    return Ok(new { fileURL });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Cores.Numbers.FiveHundred, $"Internal server error: {ex}");
            }
        }
    }
}

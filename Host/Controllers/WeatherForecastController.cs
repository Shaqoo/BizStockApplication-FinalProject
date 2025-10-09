using Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IWebHostEnvironment _environment;
        private readonly IFezService _fezService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWebHostEnvironment environment, IFezService fezService)
        {
            _logger = logger;
            _fezService = fezService;
            _environment = environment;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// Uploads and saves an application photo in wwwroot/photos.
        /// </summary>
        /// <param name="request">The photo file to upload.</param>
        /// <returns>The relative URL to the uploaded photo.</returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadPhoto([FromForm] PhotoUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            // Fallback to current directory + "wwwroot" if WebRootPath is null
            var webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Ensure upload directory exists
            var uploadPath = Path.Combine(webRootPath, "photos");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate a unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            // Return relative URL
            var fileUrl = $"/photos/{fileName}";
            return Ok(fileUrl);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders(
       [FromQuery] DateTime? startDate,
       [FromQuery] DateTime? endDate)
        {
            var start = startDate ?? DateTime.UtcNow.AddDays(-7);
            var end = endDate ?? DateTime.UtcNow;

            var result = await _fezService.GetOrdersByStatusAsync(start, end);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                count = result.Data.Count,
                orders = result.Data.Select(o => new
                {
                    o.OrderNo,
                    o.RecipientName,
                    o.OrderStatus,
                    o.Cost,
                    o.OrderDate
                })
            });

        }
        public class PhotoUploadRequest
        {
            [Required]
            public IFormFile File { get; set; } = default!;
        }
    }
}


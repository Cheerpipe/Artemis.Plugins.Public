using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artemis.Core.Services;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using RGB.NET.Core;
using Serilog;

namespace Artemis.Plugins.ExtendedWebAPI.Controllers
{
    internal class ColorController : WebApiController
    {
        private IRgbService _rgbService;
        private ILogger _logger;
        public ColorController(IRgbService rgbService, ILogger logger)
        {
            _rgbService = rgbService;
            _logger = logger;
        }

        [Route(HttpVerbs.Get, "/extended-rest-api/get-led-color/{deviceType}/{ledId}")]
        public async Task GetCurrentVibrantColor(string deviceType, string ledId)
        {
            if (!(Enum.TryParse(typeof(RGBDeviceType), deviceType, true, out object parsedType)))
            {
                string message = $"Device type {deviceType} don't exists";
                _logger.Information(message);
                throw HttpException.NotFound(message);
            }
                
            if (!(Enum.TryParse(typeof(LedId), ledId, true, out object parsedLedId)))
            {
                string message = $"Led Id {ledId} don't exists";
                _logger.Information(message);
                throw HttpException.NotFound(message);
            }

            var device = _rgbService.Devices.FirstOrDefault(d => d.DeviceType == (RGBDeviceType)parsedType!);
            if (device == null)
            {
                string message = $"Device type {ledId} not found";
                _logger.Information(message);
                throw HttpException.NotFound(message);
            }

            var led = device.RgbDevice.Surface!.Leds.FirstOrDefault(l => l.Id == (LedId)parsedLedId!);
            if (led == null)
                if (device == null)
                {
                    string message = $"Led Id {ledId} not found";
                    _logger.Information(message);
                    throw HttpException.NotFound(message);
                }

            HttpContext.Response.ContentType = "text/plain";
            await using var writer = HttpContext.OpenResponseText(new UTF8Encoding(false));
            await writer.WriteAsync(led!.Color.AsRGBHexString());
        }
    }
}
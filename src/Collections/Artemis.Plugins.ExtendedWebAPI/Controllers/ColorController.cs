using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artemis.Core.Services;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using RGB.NET.Core;

namespace Artemis.Plugins.ExtendedWebAPI.Controllers
{
    internal class ColorController : WebApiController
    {
        private IRgbService _rgbService;
        public ColorController(IRgbService rgbService)
        {
            _rgbService = rgbService;
        }

        [Route(HttpVerbs.Get, "/extended-rest-api/get-led-color/{deviceType}/{ledId}")]
        public async Task GetCurrentVibrantColor(string deviceType, string ledId)
        {
            if (!(Enum.TryParse(typeof(RGBDeviceType), deviceType, true, out object parsedType)))
                throw HttpException.NotFound($"Device type {deviceType} don't exists");

            if (!(Enum.TryParse(typeof(LedId), ledId, true, out object parsedLedId)))
                throw HttpException.NotFound($"Led Id {ledId} don't exists");

            //RGBDeviceType.Keyboard
            //LedId.Keyboard_Space;
            //LedId.Keyboard_G;

            var device = _rgbService.Devices.FirstOrDefault(d => d.DeviceType == (RGBDeviceType)parsedType!);
            if (device == null)
                throw HttpException.NotFound($"Device type {ledId} not found");

            var led = device.RgbDevice.Surface!.Leds.FirstOrDefault(l => l.Id == (LedId)parsedLedId!);
            if (led == null)
                throw HttpException.NotFound($"Led Id {ledId} not found");

            HttpContext.Response.ContentType = "text/plain";
            await using var writer = HttpContext.OpenResponseText(new UTF8Encoding(false));
            await writer.WriteAsync(led.Color.AsRGBHexString());
        }
    }
}
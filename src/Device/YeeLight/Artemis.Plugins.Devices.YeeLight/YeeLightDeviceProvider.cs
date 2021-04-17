using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using Serilog;

namespace Artemis.Plugins.Devices.YeeLight
{
    public class YeeLightDeviceProvider : DeviceProvider
    {
        private readonly ILogger _logger;
        private readonly IRgbService _rgbService;

        public YeeLightDeviceProvider(IRgbService rgbService, ILogger logger) : base(RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance)
        {
            _rgbService = rgbService;
            _logger = logger;
        }

        public override void Enable()
        {
            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }

        public override void Disable()
        {
            RGB.NET.Devices.YeeLight.YeeLightDeviceProvider yeeLightProvider = (RGB.NET.Devices.YeeLight.YeeLightDeviceProvider)RgbDeviceProvider;
            yeeLightProvider.Dispose();
            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
        }
    }
}
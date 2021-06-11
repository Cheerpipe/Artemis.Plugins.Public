using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using Serilog;

namespace Artemis.Plugins.Devices.YeeLight
{
    public class YeeLightDeviceProvider : DeviceProvider
    {
        private readonly IRgbService _rgbService;

        public YeeLightDeviceProvider(IRgbService rgbService) : base(RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance)
        {
            _rgbService = rgbService;
        }

        public override void Enable()
        {
            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }

        public override void Disable()
        {
            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
            RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance.Dispose();
        }
    }
}
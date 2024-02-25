using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using RGB.NET.Core;

namespace Artemis.Plugins.Devices.PowerPlay
{
    public class PowerPlayDeviceProvider : DeviceProvider
    {
        private readonly IDeviceService _rgbService;

        public PowerPlayDeviceProvider(IDeviceService rgbService)
        {
            _rgbService = rgbService;
        }

        public override void Enable()
        {
            _rgbService.AddDeviceProvider(this);
        }

        public override void Disable()
        {
            _rgbService.RemoveDeviceProvider(this);
            RgbDeviceProvider.Dispose();
        }

        public override IRGBDeviceProvider RgbDeviceProvider  => RGB.NET.Devices.PowerPlay.PowerPlayDeviceProvider.Instance;
    }
}
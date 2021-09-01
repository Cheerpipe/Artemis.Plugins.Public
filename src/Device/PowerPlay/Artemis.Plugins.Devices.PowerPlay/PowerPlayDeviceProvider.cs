using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;

namespace Artemis.Plugins.Devices.PowerPlay
{
    public class PowerPlayDeviceProvider : DeviceProvider
    {
        private readonly IRgbService _rgbService;

        public PowerPlayDeviceProvider(IRgbService rgbService) : base(RGB.NET.Devices.PowerPlay.PowerPlayDeviceProvider.Instance)
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
            RgbDeviceProvider.Dispose();
        }
    }
}
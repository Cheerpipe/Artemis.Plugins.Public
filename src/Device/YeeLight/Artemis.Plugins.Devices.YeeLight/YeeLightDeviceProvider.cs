using Artemis.Core;
using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using RGB.NET.Devices.YeeLight;
using System.Collections.Generic;
using System.Linq;

namespace Artemis.Plugins.Devices.YeeLight
{
    [PluginFeature(Name ="Yeelight Device Provider")]
    public class YeeLightDeviceProvider : DeviceProvider
    {
        private readonly IRgbService _rgbService;

        private readonly PluginSetting<bool> _useAutomaticScan;
        private readonly PluginSetting<bool> _useAllAvailableMulticastAddresses;
        private readonly PluginSetting<List<YeeLightDeviceDefinition>> _yeeLightDeviceDefinitions;

        public YeeLightDeviceProvider(IRgbService rgbService, PluginSettings pluginSettings) : base(RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance)
        {
            _rgbService = rgbService;
            _useAllAvailableMulticastAddresses = pluginSettings.GetSetting("UseAllAvailableMulticastAddresses", false);
            _yeeLightDeviceDefinitions = pluginSettings.GetSetting("YeeLightDeviceDefinitions", new List<YeeLightDeviceDefinition>());
            _useAutomaticScan = pluginSettings.GetSetting("UseAutomaticScan", true);
        }

        public override void Enable()
        {
            RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance.YeeLightDeviceDefinitions = _yeeLightDeviceDefinitions.Value.ToList();
            RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance.UseAllAvailableMulticastAddresses = _useAllAvailableMulticastAddresses.Value;
            RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance.UseAutomaticScan = _useAutomaticScan.Value;
            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }

        public override void Disable()
        {
            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
            RgbDeviceProvider.Dispose();
        }
    }
}
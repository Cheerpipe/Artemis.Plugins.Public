using Artemis.Core;
using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using RGB.NET.Devices.YeeLight;
using RGB.NET.Devices.YeeLight.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Artemis.Plugins.Devices.YeeLight
{
    [PluginFeature(Name ="Yeelight Device Provider")]
    public class YeeLightDeviceProvider : DeviceProvider
    {
        private readonly IRgbService _rgbService;

        private PluginSetting<bool> _useAllAvailableMulticastAddresses;
        private PluginSetting<List<YeeLightDeviceDefinition>> _yeeLightDeviceDefinitions;
        private PluginSetting<ScanMode> _scanMode;

        public YeeLightDeviceProvider(IRgbService rgbService, PluginSettings pluginSettings) : base(RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance)
        {
            _rgbService = rgbService;
            _useAllAvailableMulticastAddresses = pluginSettings.GetSetting("UseAllAvailableMulticastAddresses", false);
            _yeeLightDeviceDefinitions = pluginSettings.GetSetting("YeeLightDeviceDefinitionsSetting", new List<YeeLightDeviceDefinition>());
            _scanMode = pluginSettings.GetSetting("ScanMode", ScanMode.Automatic);
        }

        public override void Enable()
        {
            RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance.YeeLightDeviceDefinitions = _yeeLightDeviceDefinitions.Value.ToList();
            RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance.UseAllAvailableMulticastAddresses = _useAllAvailableMulticastAddresses.Value;
            RGB.NET.Devices.YeeLight.YeeLightDeviceProvider.Instance.ScanMode = _scanMode.Value;
            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }

        public override void Disable()
        {
            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
            RgbDeviceProvider.Dispose();
        }
    }
}
using Artemis.Core;
using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using RGB.NET.Devices.Ledenet;
using System.Collections.Generic;
using System.Linq;

namespace Artemis.Plugins.Devices.Ledenet
{
    public class LedenetDeviceProvider : DeviceProvider
    {
        private readonly IRgbService _rgbService;

        private readonly PluginSetting<List<LedenetDeviceDefinition>> _ledenetDeviceDefinitions;

        public LedenetDeviceProvider(IRgbService rgbService, PluginSettings pluginSettings) : base(RGB.NET.Devices.Ledenet.LedenetDeviceProvider.Instance)
        {
            _rgbService = rgbService;
            _ledenetDeviceDefinitions = pluginSettings.GetSetting("LedenetDeviceDefinitionsSetting", new List<LedenetDeviceDefinition>());
        }

        public override void Enable()
        {
            RGB.NET.Devices.Ledenet.LedenetDeviceProvider.Instance.LedenetDeviceDefinitions = _ledenetDeviceDefinitions.Value.ToList();
            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }

        public override void Disable()
        {
            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
            RgbDeviceProvider.Dispose();
        }
    }
}
using Artemis.Core;
using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;
using RGB.NET.Devices.Adalight;
using System.Collections.Generic;

namespace Artemis.Plugins.Devices.Adalight
{
    public class AdalightDeviceProvider : DeviceProvider
    {
        private readonly IRgbService _rgbService;

        private readonly PluginSetting<List<AdalightDeviceDefinition>> _adalightDeviceDefinitions;

        public AdalightDeviceProvider(IRgbService rgbService, PluginSettings pluginSettings) : base(RGB.NET.Devices.Adalight.AdalightDeviceProvider.Instance)
        {
            _rgbService = rgbService;
            _adalightDeviceDefinitions = pluginSettings.GetSetting("AdalightDeviceDefinitionsSetting", new List<AdalightDeviceDefinition>());
        }

        public override void Enable()
        {
            foreach (var def in _adalightDeviceDefinitions.Value)
            {
                RGB.NET.Devices.Adalight.AdalightDeviceProvider.Instance.AddDeviceDefinition(def);
            }
            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }

        public override void Disable()
        {
            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
            RgbDeviceProvider.Dispose();
        }
    }
}
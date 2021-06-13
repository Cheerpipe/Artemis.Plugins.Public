using Artemis.Core;
using Artemis.Core.Services;
using Artemis.UI.Shared;
using RGB.NET.Devices.Ledenet;
using Stylet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artemis.Plugins.Devices.Ledenet
{

    public class LedenetConfigurationDialogViewModel : PluginConfigurationViewModel
    {
        private readonly IPluginManagementService _pluginManagementService;

        private readonly PluginSetting<List<LedenetDeviceDefinition>> _ledenetDeviceDefinitionsSetting;
        private List<LedenetDeviceDefinition> _ledenetDeviceDefinitions;

        public BindableCollection<LedenetDeviceDefinition> Definitions { get; }

        public LedenetConfigurationDialogViewModel(Plugin plugin, PluginSettings settings, IPluginManagementService pluginManagementService) : base(plugin)
        {
            _pluginManagementService = pluginManagementService;

            _ledenetDeviceDefinitionsSetting = settings.GetSetting("LedenetDeviceDefinitionsSetting", new List<LedenetDeviceDefinition>());
            _ledenetDeviceDefinitions = _ledenetDeviceDefinitionsSetting.Value;

            Definitions = new BindableCollection<LedenetDeviceDefinition>(_ledenetDeviceDefinitions);
        }

        public void AddDefinition()
        {
            Definitions.Add(new LedenetDeviceDefinition());
        }

        public void DeleteRow(object def)
        {
            if (def is LedenetDeviceDefinition deviceDefinition)
            {
                Definitions.Remove(deviceDefinition);
            }
        }

        public BindableCollection<ValueDescription> ScanModes { get; }

        protected override void OnClose()
        {

            _ledenetDeviceDefinitionsSetting.Value.Clear();
            _ledenetDeviceDefinitionsSetting.Value.AddRange(Definitions.Where(d => !string.IsNullOrWhiteSpace(d.HostName)));
            _ledenetDeviceDefinitionsSetting.Save();

            Task.Run(() =>
            {
                LedenetDeviceProvider deviceProvider = Plugin.GetFeature<LedenetDeviceProvider>();
                if (deviceProvider == null || !deviceProvider.IsEnabled) return;
                _pluginManagementService.DisablePluginFeature(deviceProvider, false);
                _pluginManagementService.EnablePluginFeature(deviceProvider, false);
            });
            base.OnClose();
        }
    }
}
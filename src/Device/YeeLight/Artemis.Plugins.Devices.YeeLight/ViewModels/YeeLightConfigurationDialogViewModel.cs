using Artemis.Core;
using Artemis.Core.Services;
using Artemis.UI.Shared;
using RGB.NET.Devices.YeeLight;
using RGB.NET.Devices.YeeLight.Enums;
using Stylet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artemis.Plugins.Devices.YeeLight
{

    public class YeeLightConfigurationDialogViewModel : PluginConfigurationViewModel
    {
        private readonly IPluginManagementService _pluginManagementService;


        private readonly PluginSetting<bool> _useAllAvailableMulticastAddressesSetting;
        private bool _useAllAvailableMulticastAddresses;
        public bool UseAllAvailableMulticastAddresses
        {
            get => _useAllAvailableMulticastAddresses;
            set => SetAndNotify(ref _useAllAvailableMulticastAddresses, value);
        }

        private readonly PluginSetting<List<YeeLightDeviceDefinition>> _yeeLightDeviceDefinitionsSetting;
        private List<YeeLightDeviceDefinition> _yeeLightDeviceDefinitions;

        public BindableCollection<YeeLightDeviceDefinition> Definitions { get; }

        private readonly PluginSetting<ScanMode> _scanModeSetting;
        private ScanMode _scanMode;

        public ScanMode SelectedScanMode
        {
            get => _scanMode;
            set
            {
                SetAndNotify(ref _scanMode, value);
                EnableManualInput = _scanMode == ScanMode.Manual;
                EnableAutomaticInput = _scanMode == ScanMode.Automatic;
            }
        }

        public YeeLightConfigurationDialogViewModel(Plugin plugin, PluginSettings settings, IPluginManagementService pluginManagementService) : base(plugin)
        {
            _pluginManagementService = pluginManagementService;

            _useAllAvailableMulticastAddressesSetting = settings.GetSetting("UseAllAvailableMulticastAddresses", false);
            _useAllAvailableMulticastAddresses = _useAllAvailableMulticastAddressesSetting.Value;

            _yeeLightDeviceDefinitionsSetting = settings.GetSetting("YeeLightDeviceDefinitionsSetting", new List<YeeLightDeviceDefinition>());
            _yeeLightDeviceDefinitions = _yeeLightDeviceDefinitionsSetting.Value;

            _scanModeSetting = settings.GetSetting("ScanMode", ScanMode.Automatic);
            _scanMode = _scanModeSetting.Value;

            Definitions = new BindableCollection<YeeLightDeviceDefinition>(_yeeLightDeviceDefinitions);
            ScanModes = new BindableCollection<ValueDescription>(EnumUtilities.GetAllValuesAndDescriptions(typeof(ScanMode)));

            _enableAutomaticInput = _scanMode == ScanMode.Automatic;
            _enableManualInput = _scanMode == ScanMode.Manual;
        }

        public void AddDefinition()
        {
            Definitions.Add(new YeeLightDeviceDefinition());
        }

        private bool _enableManualInput;
        public bool EnableManualInput
        {
            get => _enableManualInput;
            set => SetAndNotify(ref _enableManualInput, value);
        }

        private bool _enableAutomaticInput;
        public bool EnableAutomaticInput
        {
            get => _enableAutomaticInput;
            set => SetAndNotify(ref _enableAutomaticInput, value);
        }

        public void DeleteRow(object def)
        {
            if (def is YeeLightDeviceDefinition deviceDefinition)
            {
                Definitions.Remove(deviceDefinition);
            }
        }

        public BindableCollection<ValueDescription> ScanModes { get; }


        protected override void OnClose()
        {
            _useAllAvailableMulticastAddressesSetting.Value = UseAllAvailableMulticastAddresses;
            _useAllAvailableMulticastAddressesSetting.Save();

            _scanModeSetting.Value = SelectedScanMode;
            _scanModeSetting.Save();


            _yeeLightDeviceDefinitionsSetting.Value.Clear();
            _yeeLightDeviceDefinitionsSetting.Value.AddRange(Definitions.Where(d => !string.IsNullOrWhiteSpace(d.HostName)));
            _yeeLightDeviceDefinitionsSetting.Save();


            Task.Run(() =>
            {
                YeeLightDeviceProvider deviceProvider = Plugin.GetFeature<YeeLightDeviceProvider>();
                if (deviceProvider == null || !deviceProvider.IsEnabled) return;
                _pluginManagementService.DisablePluginFeature(deviceProvider, false);
                _pluginManagementService.EnablePluginFeature(deviceProvider, false);
            });
            base.OnClose();
        }
    }
}
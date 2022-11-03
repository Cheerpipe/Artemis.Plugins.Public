using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Artemis.Core;
using Artemis.Core.Services;
using Artemis.Plugins.Devices.YeeLight.ViewModels.Dialogs;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Services;
using ReactiveUI;
using RGB.NET.Devices.YeeLight;

namespace Artemis.Plugins.Devices.YeeLight.ViewModels
{

    public class YeeLightConfigurationViewModel : PluginConfigurationViewModel
    {
        #region Variables definitions

        private readonly IPluginManagementService _pluginManagementService;
        private readonly IWindowService _windowService;
        private readonly PluginSetting<List<YeeLightDeviceDefinition>> _definitions;
        private readonly PluginSettings _settings;

        #endregion

        #region Constructor

        public YeeLightConfigurationViewModel(Plugin plugin, PluginSettings settings, IWindowService windowService, IPluginManagementService pluginManagementService)
            : base(plugin)
        {
            _pluginManagementService = pluginManagementService;
            _windowService = windowService;
            _settings = settings;

            _definitions = settings.GetSetting("YeeLightDeviceDefinitions", new List<YeeLightDeviceDefinition>());
            Definitions = new ObservableCollection<YeeLightDeviceDefinition>(_definitions.Value);
            TurnOffLedsOnShutdown = settings.GetSetting("TurnOffLedsOnShutdown", false);
            UseAutomaticScan = settings.GetSetting("UseAutomaticScan", true);
            UseAllAvailableMulticastAddresses = settings.GetSetting("UseAllAvailableMulticastAddresses", false);

            AddDevice = ReactiveCommand.CreateFromTask(ExecuteAddDevice);
            EditDevice = ReactiveCommand.CreateFromTask<YeeLightDeviceDefinition>(ExecuteEditDevice);
            RemoveDevice = ReactiveCommand.Create<YeeLightDeviceDefinition>(ExecuteRemoveDevice);
            Save = ReactiveCommand.Create(ExecuteSave);
            Cancel = ReactiveCommand.CreateFromTask(ExecuteCancel);
        }

        #endregion

        #region Settings

        public ObservableCollection<YeeLightDeviceDefinition> Definitions { get; }
        public PluginSetting<bool> TurnOffLedsOnShutdown { get; }
        public PluginSetting<bool> UseAllAvailableMulticastAddresses { get; }
        public PluginSetting<bool> UseAutomaticScan { get; }

        #endregion

        #region Commands definitions

        public ReactiveCommand<Unit, Unit> AddDevice { get; }
        public ReactiveCommand<YeeLightDeviceDefinition, Unit> EditDevice { get; }
        public ReactiveCommand<YeeLightDeviceDefinition, Unit> RemoveDevice { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }

        #endregion

        #region Commands methods

        private async Task ExecuteCancel()
        {
            if (TurnOffLedsOnShutdown.HasChanged ||
                UseAllAvailableMulticastAddresses.HasChanged ||
                UseAutomaticScan.HasChanged ||
                _definitions.HasChanged)
            {
                if (!await _windowService.ShowConfirmContentDialog("Discard changes", "Do you want to discard any changes you made?"))
                    return;
            }

            _definitions.RejectChanges();
            TurnOffLedsOnShutdown.RejectChanges();
            UseAllAvailableMulticastAddresses.RejectChanges();
            UseAutomaticScan.RejectChanges();

            Close();
        }

        private void ExecuteSave()
        {
            _definitions.Save();
            TurnOffLedsOnShutdown.Save();
            UseAutomaticScan.Save();
            UseAllAvailableMulticastAddresses.Save();

            // Fire & forget re-enabling the plugin
            Task.Run(() =>
            {
                YeeLightDeviceProvider deviceProvider = Plugin.GetFeature<YeeLightDeviceProvider>();
                if (deviceProvider == null || !deviceProvider.IsEnabled) return;
                _pluginManagementService.DisablePluginFeature(deviceProvider, false);
                _pluginManagementService.EnablePluginFeature(deviceProvider, false);
            });

            Close();
        }

        private void ExecuteRemoveDevice(YeeLightDeviceDefinition device)
        {
            _definitions.Value.Remove(device);
            Definitions.Remove(device);
        }

        private async Task ExecuteEditDevice(YeeLightDeviceDefinition device)
        {
            if (await _windowService.ShowDialogAsync<DeviceConfigurationDialogViewModel, DeviceDialogResult>(("device", device)) == DeviceDialogResult.Remove)
                Definitions.Remove(device);
        }

        private async Task ExecuteAddDevice()
        {
            YeeLightDeviceDefinition device = new()
            {
                DeviceName = String.Empty,
                HostName = String.Empty,
                Model = YeelightAPI.Models.MODEL.Color,
                UseMusicMode = true,
            };

            if (await _windowService.ShowDialogAsync<DeviceConfigurationDialogViewModel, DeviceDialogResult>(("device", device)) != DeviceDialogResult.Save)
                return;

            _definitions.Value.Add(device);
            Definitions.Add(device);
        }

        #endregion

    }
}
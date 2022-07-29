using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Artemis.Core;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Services;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using RGB.NET.Devices.YeeLight;
using RGB.NET.Devices.YeeLight.Enums;
using YeelightAPI.Models;

namespace Artemis.Plugins.Devices.YeeLight.ViewModels.Dialogs
{
    public class DeviceConfigurationDialogViewModel : DialogViewModelBase<DeviceDialogResult>
    {
        private readonly YeeLightDeviceDefinition _device;
        private readonly PluginSettings _settings;
        private readonly IWindowService _windowService;
        private string _deviceName;
        private string _hostname;
        private MODEL _model;
        private bool _useMusicMode;
        private bool _hasChanges;

        public DeviceConfigurationDialogViewModel(YeeLightDeviceDefinition device, PluginSettings settings, IWindowService windowService)
        {
            _device = device;
            _settings = settings;
            _windowService = windowService;

            _device = device;
            _deviceName = _device.DeviceName;
            _hostname = _device.HostName;
            _model = _device.Model;
            _useMusicMode = _device.UseMusicMode;

            this.ValidationRule(vm => vm.DeviceName, v => !string.IsNullOrWhiteSpace(v), "A device name is required");
            this.ValidationRule(vm => vm.HostName, v => !string.IsNullOrWhiteSpace(v), "A hostname is required");

            Save = ReactiveCommand.Create(ExecuteSave, ValidationContext.Valid);
            Cancel = ReactiveCommand.CreateFromTask(ExecuteCancel);
            RemoveDevice = ReactiveCommand.CreateFromTask(ExecuteRemoveDevice);
            PropertyChanged += (_, _) => _hasChanges = true;
        }

        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> RemoveDevice { get; }

        public List<MODEL> Modelos { get => Enum.GetValues(typeof(MODEL)).Cast<MODEL>().ToList(); }

        public YeeLightDeviceDefinition Device { get; }

        public string DeviceName
        {
            get => _deviceName;
            set => RaiseAndSetIfChanged(ref _deviceName, value);
        }

        public string HostName
        {
            get => _hostname;
            set => RaiseAndSetIfChanged(ref _hostname, value);
        }

        public MODEL Model
        {
            get => _model;
            set => RaiseAndSetIfChanged(ref _model, value);
        }

        public bool UseMusicMode
        {
            get => _useMusicMode;
            set => RaiseAndSetIfChanged(ref _useMusicMode, value);
        }

        private void ExecuteSave()
        {
            if (HasErrors)
                return;

            _device.DeviceName = DeviceName;
            _device.HostName = HostName;
            _device.Model = Model;
            _device.UseMusicMode = UseMusicMode;
            _device.Model = Model;
            Close(DeviceDialogResult.Save);
        }

        private async Task ExecuteCancel()
        {
            if (_hasChanges)
            {
                bool confirmed = await _windowService.ShowConfirmContentDialog("Discard changes", "Are you sure you want to discard your changes?");
                if (confirmed)
                    Close(DeviceDialogResult.Cancel);
            }
            else
                Close(DeviceDialogResult.Cancel);
        }

        private async Task ExecuteRemoveDevice()
        {
            bool confirmed = await _windowService.ShowConfirmContentDialog("Remove device", "Are you sure you want to remove this device?");
            if (!confirmed)
                return;

            PluginSetting<List<YeeLightDeviceDefinition>> definitions = _settings.GetSetting("YeeLightDeviceDefinitions", new List<YeeLightDeviceDefinition>());
            definitions.Value.Remove(_device);
            Close(DeviceDialogResult.Remove);
        }
    }

    public enum DeviceDialogResult
    {
        Save,
        Cancel,
        Remove
    }
}
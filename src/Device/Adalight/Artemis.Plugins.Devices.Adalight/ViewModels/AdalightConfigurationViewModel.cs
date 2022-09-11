using Artemis.Core;
using Artemis.Core.Services;
using Artemis.UI.Shared;
using Avalonia.Markup.Xaml;
using RGB.NET.Devices.Adalight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Artemis.Plugins.Devices.Adalight.ViewModels
{
    public class AdalightConfigurationViewModel : PluginConfigurationViewModel
    {
        private readonly IPluginManagementService _pluginManagementService;
        private readonly PluginSetting<List<AdalightDeviceDefinition>> _adalightDeviceDefinitionsSetting;
        public PluginSetting<bool> TurnOffLedsOnShutdown { get; }
        private readonly List<AdalightDeviceDefinition> _adalightDeviceDefinitions;
        public ObservableCollection<AdalightDeviceDefinition> Definitions { get; }

        public AdalightConfigurationViewModel(Plugin plugin, PluginSettings settings, IPluginManagementService pluginManagementService) : base(plugin)
        {
            _pluginManagementService = pluginManagementService;
            _adalightDeviceDefinitionsSetting = settings.GetSetting("AdalightDeviceDefinitionsSetting", new List<AdalightDeviceDefinition>());
            _adalightDeviceDefinitions = _adalightDeviceDefinitionsSetting.Value;
        }

        public void AddDefinition()
        {
            _adalightDeviceDefinitions.Add(new AdalightDeviceDefinition());
        }

        public void DeleteRow(object def)
        {
            if (def is AdalightDeviceDefinition deviceDefinition)
            {
                _adalightDeviceDefinitions.Remove(deviceDefinition);
            }
        }

        public override void OnCloseRequested()
        {
            _adalightDeviceDefinitionsSetting.Value.Clear();
            _adalightDeviceDefinitionsSetting.Value.AddRange(Definitions.Where(d => !string.IsNullOrWhiteSpace(d.Name)));
            _adalightDeviceDefinitionsSetting.Save();

            Task.Run(() =>
            {
                AdalightDeviceProvider deviceProvider = Plugin.GetFeature<AdalightDeviceProvider>();
                if (deviceProvider == null || !deviceProvider.IsEnabled) return;
                _pluginManagementService.DisablePluginFeature(deviceProvider, false);
                _pluginManagementService.EnablePluginFeature(deviceProvider, false);
            });
            base.OnCloseRequested();
        }

        public class EnumToCollectionExtension : MarkupExtension
        {
            public Type EnumType { get; set; }

            public override object ProvideValue(IServiceProvider serviceProvider)
            {
                if (EnumType == null) throw new ArgumentNullException(nameof(EnumType));

                return Enum.GetValues(EnumType).Cast<Enum>().Select(EnumToDescriptionOrString);
            }

            private string EnumToDescriptionOrString(Enum value)
            {
                return value.GetType().GetField(value.ToString())
                           .GetCustomAttributes(typeof(DescriptionAttribute), false)
                           .Cast<DescriptionAttribute>()
                           .FirstOrDefault()?.Description ?? value.ToString();
            }

        }
    }
}
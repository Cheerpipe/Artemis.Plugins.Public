using Artemis.Core;
using Serilog;
using System;
using Artemis.Plugins.DataModelExpansions.DisplaySettings.DataModels;
using Microsoft.Win32;
using System.Linq;
using WindowsDisplayAPI;
using WindowsDisplayAPI.DisplayConfig;
using Artemis.Core.Modules;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Plugins.DataModelExpansions.DisplaySettings.Custom;

namespace Artemis.Plugins.DataModelExpansions.DisplaySettings
{
    public class CustomDisplayModeDataModelExpansion : Module<CustomDisplayModeDataModel>
    {

        private readonly ILogger _logger;

        public CustomDisplayModeDataModelExpansion(PluginSettings settings, ILogger logger)
        {
            _logger = logger;
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            UpdateDataDataModel();
        }

        public override List<IModuleActivationRequirement> ActivationRequirements => null;

        public override void Enable()
        {
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            SystemEvents_DisplaySettingsChanged(null, null);
        }

        public override void Disable()
        {
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
        }

        public override void Update(double deltaTime) { }

        private void UpdateDataDataModel()
        {
            Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(500);
                    DataModel.DisplayMode = CustomDisplayModeDetector.GetCurrentMode();
                }
                catch (Exception e)
                {
                    _logger.Error(e.ToString());
                }
            });
        }
    }
}
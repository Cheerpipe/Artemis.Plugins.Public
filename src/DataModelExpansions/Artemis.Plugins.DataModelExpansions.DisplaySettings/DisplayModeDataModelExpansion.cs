using Serilog;
using System;
using Artemis.Plugins.DataModelExpansions.DisplaySettings.DataModels;
using Microsoft.Win32;
using System.Linq;
using WindowsDisplayAPI;
using WindowsDisplayAPI.DisplayConfig;
using Artemis.Core.Modules;
using System.Collections.Generic;

namespace Artemis.Plugins.DataModelExpansions.DisplaySettings
{
    public class DisplayModeDataModelExpansion : Module<DisplaySettingsDataModel>
    {
        private readonly ILogger _logger;

        public DisplayModeDataModelExpansion(ILogger logger)
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
            try
            {
                DataModel.Topology = PathInfo.GetCurrentTopology();

                var displayList = Display.GetDisplays().ToList();
                DataModel.DisplayCount = displayList.Count;

                DataModel.Displays.ClearDynamicChildren();
                for (int i = 0; i < displayList.Count; i++)
                {

                    DataModel.Displays.AddDynamicChild(
                        $"[{i}] - {displayList[i].DeviceName}",
                        new DisplaySettingDataModel()
                        {
                            IsPrimary = displayList[i].IsGDIPrimary,
                            Name = displayList[i].DeviceName,
                            Number = i,
                            Adapter = displayList[i].Adapter.DeviceName,
                            RefreshRate = displayList[i].CurrentSetting.Frequency,
                            Resolution = displayList[i].CurrentSetting.Resolution,
                            Position = displayList[i].CurrentSetting.Position,
                            ColorDepth = displayList[i].CurrentSetting.ColorDepth,
                            DisplayRotation = displayList[i].CurrentSetting.Orientation,
                        }
                    );
                }

            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }
    }
}
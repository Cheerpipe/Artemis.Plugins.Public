using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using Artemis.Core.Services;
using Serilog;
using System;
using System.Threading.Tasks;
using Artemis.Plugins.DataModelExpansions.DisplaySettings.DataModels;
using Microsoft.Win32;
using System.Linq;
using WindowsDisplayAPI;
using WindowsDisplayAPI.DisplayConfig;

namespace Artemis.Plugins.DataModelExpansions.DisplaySettings
{
    public class DisplayModeDataModelExpansion : DataModelExpansion<DisplaySettingsDataModel>
    {

        private readonly ILogger _logger;

        public DisplayModeDataModelExpansion(PluginSettings settings, ILogger logger, IColorQuantizerService colorQuantizer)
        {
            _logger = logger;
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            //Task task = Task.Run(async () => await UpdateDataDataModel());
            UpdateDataDataModel();
        }

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
        //private async Task UpdateDataDataModel()
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
                        },
                        string.Format("[{0}] - {1}", i, displayList[i].DeviceName),
                        string.Format("[{0}] - {1}", i, displayList[i].DeviceName)
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
using System;
using System.Collections.Generic;
using System.Windows;
using Artemis.Core.Modules;
using Artemis.Plugins.DataModelExpansions.PowerState.DataModels;
using Artemis.Plugins.DataModelExpansions.PowerState.Utils;
using Microsoft.Win32;
using Serilog;

namespace Artemis.Plugins.DataModelExpansions.PowerState
{
    public class PowerStateDataModelExpansion : Module<PowerStateDataModel>
    {

        private readonly ILogger _logger;
        private readonly PowerPlanUtil _powerPlanUtil;

        public PowerStateDataModelExpansion(ILogger logger)
        {
            _logger = logger;
            _powerPlanUtil = new PowerPlanUtil();
        }

        public override List<IModuleActivationRequirement> ActivationRequirements => null;

        public override void Enable()
        {
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            _powerPlanUtil.PowerPlanChanged += _powerPlanUtil_PowerPlanChanged;
            _powerPlanUtil.StartPlanWatcher();
            UpdatePowerState();
            UpdatePowerPlan();
        }

        private void _powerPlanUtil_PowerPlanChanged(object sender, EventArgs e)
        {
            UpdatePowerPlan();
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            UpdatePowerState();
        }

        public override void Disable()
        {
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;

            _powerPlanUtil.StopPlanWatcher();
            _powerPlanUtil.PowerPlanChanged -= _powerPlanUtil_PowerPlanChanged;
        }

        public override void Update(double deltaTime) { }

        private void UpdatePowerState()
        {
            try
            {
                DataModel.UsingBattery = SystemParameters.PowerLineStatus == PowerLineStatus.Offline;
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }

        private void UpdatePowerPlan()
        {
            try
            {
                DataModel.CurrentPowerPlanGuid = _powerPlanUtil.GetCurrentPowerPlanGuid();

                // We will use W32 api to get Friendly Name because this method is localization friendly whereas registry not
                DataModel.CurrentPowerPlanFriendlyName = PowerPlanUtilesW32.GetCurrentPlanFriendlyname();
                DataModel.PowerPlanChanged.Trigger(new PowerPlanEventArgs(DataModel.CurrentPowerPlanGuid, DataModel.CurrentPowerPlanFriendlyName));
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }
    }
}
using System;
using System.Globalization;
using System.Linq;
using System.Management;

namespace Artemis.Plugins.DataModelExpansions.PowerPlan.Utils
{
    public class PowerPlanUtil : IDisposable
    {
        private const string ActivePlanKeyName = @"SYSTEM\\CurrentControlSet\\Control\\Power\\User\\PowerSchemes";
        private const string ActivePlanKeyValueName = "ActivePowerScheme";
        private ManagementEventWatcher _currentPowerPlanWatcher;

        public event EventHandler PowerPlanChanged;

        public static Guid GetCurrentPowerPlanGuid()
        {
            // ReSharper disable once InconsistentNaming
            using var HKLM = Microsoft.Win32.Registry.LocalMachine;
            string sGuid = HKLM.OpenSubKey(ActivePlanKeyName)?.GetValue(ActivePlanKeyValueName)?.ToString();
            return sGuid != null ? Guid.Parse(sGuid) : Guid.Empty;
        }

        public static string GetCurrentPowerPlanFriendlyName()
        {
            // ReSharper disable once InconsistentNaming
            using var HKLM = Microsoft.Win32.Registry.LocalMachine;
            var activePlanFriendlyNameKeyName = $@"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes\{GetCurrentPowerPlanGuid()}";
            return HKLM.OpenSubKey(activePlanFriendlyNameKeyName, false)?.GetValue("FriendlyName")?.ToString()?.Split(',').Last();
        }

        public void StartPlanWatcher()
        {
            string currentPowerPlanWatcherQuery = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_LOCAL_MACHINE' AND KeyPath = '{0}' AND ValueName = '{1}'",
                ActivePlanKeyName,
                ActivePlanKeyValueName);

            _currentPowerPlanWatcher = new ManagementEventWatcher(currentPowerPlanWatcherQuery);
            _currentPowerPlanWatcher.EventArrived += CurrentPowerPlanWatcher_EventArrived;
            _currentPowerPlanWatcher.Start();
        }

        public void StopPlanWatcher()
        {
            _currentPowerPlanWatcher?.Stop();
            _currentPowerPlanWatcher?.Dispose();
        }

        private void CurrentPowerPlanWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            PowerPlanChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            StopPlanWatcher();
        }
    }
}

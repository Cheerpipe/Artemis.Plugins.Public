using System;
using Artemis.Core.Modules;
using Artemis.Core;

namespace Artemis.Plugins.DataModelExpansions.PowerState.DataModels
{
    public class PowerStateDataModel : DataModel
    {
        // Power Status
        public bool UsingBattery { get; set; }

        // Power Plan
        public Guid CurrentPowerPlanGuid { get; set; }
        public string CurrentPowerPlanFriendlyName { get; set; }
        public DataModelEvent<PowerPlanEventArgs> PowerPlanChanged { get; set; } = new();

        //TODO: Battery status
        // https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-battery?redirectedfrom=MSDN
    }

    public class PowerPlanEventArgs : DataModelEventArgs
    {
        public PowerPlanEventArgs(Guid powerPlanGuid, string powerPlanFriendlyName)
        {
            PowerPlanGuid = powerPlanGuid;
            PowerPlanFriendlyName = powerPlanFriendlyName;
        }

        public Guid PowerPlanGuid { get; set; }

        public string PowerPlanFriendlyName { get; set; }

    }
}
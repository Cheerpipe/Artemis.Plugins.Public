using Artemis.Core;
using Artemis.UI.Shared;

namespace Artemis.Plugins.Devices.Ledenet
{
    public class LedenetBootstrapper : PluginBootstrapper
    {
        public override void OnPluginLoaded(Plugin plugin)
        {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<LedenetConfigurationDialogViewModel>();
        }
    }
}

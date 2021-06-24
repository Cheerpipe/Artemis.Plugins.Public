using Artemis.Core;
using Artemis.UI.Shared;

namespace Artemis.Plugins.Devices.Adalight
{
    public class AdalightBootstrapper : PluginBootstrapper
    {
        public override void OnPluginLoaded(Plugin plugin)
        {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<AdalightConfigurationDialogViewModel>();
        }
    }
}

using Artemis.Core;
using Artemis.Plugins.Devices.Adalight.ViewModels;
using Artemis.UI.Shared;

namespace Artemis.Plugins.Devices.Adalight
{
    public class AdalightBootstrapper : PluginBootstrapper
    {
        public override void OnPluginLoaded(Plugin plugin)
        {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<AdalightConfigurationViewModel>();
        }
    }
}

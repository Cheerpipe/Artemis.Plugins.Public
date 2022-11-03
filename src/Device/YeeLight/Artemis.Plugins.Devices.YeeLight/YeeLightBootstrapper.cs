using Artemis.Core;
using Artemis.Plugins.Devices.YeeLight.ViewModels;
using Artemis.UI.Shared;

namespace Artemis.Plugins.Devices.YeeLight
{
    public class YeeLightBootstrapper : PluginBootstrapper
    {
        public override void OnPluginLoaded(Plugin plugin)
        {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<YeeLightConfigurationViewModel>();
        }
    }
}

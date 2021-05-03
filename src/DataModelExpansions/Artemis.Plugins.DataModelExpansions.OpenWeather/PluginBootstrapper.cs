using Artemis.Core;
using Artemis.UI.Shared;

namespace Artemis.Plugins.DataModelExpansions.OpenWeather
{
    public class Bootstrapper : PluginBootstrapper
    {
        public new void OnPluginLoaded(Plugin plugin)
        {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<OpenWeatherPluginConfigurationViewModel>();
        }
    }
}

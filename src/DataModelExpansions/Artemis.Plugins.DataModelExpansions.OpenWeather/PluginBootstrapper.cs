using Artemis.Core;
using Artemis.Plugins.DataModelExpansions.OpenWeather.ViewModels;
using Artemis.UI.Shared;

namespace Artemis.Plugins.DataModelExpansions.OpenWeather
{
    public class OpenWeatherBootstrapper : PluginBootstrapper
    {
        public override void OnPluginLoaded(Plugin plugin)
        {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<OpenWeatherPluginConfigurationViewModel>();
        }
    }
}

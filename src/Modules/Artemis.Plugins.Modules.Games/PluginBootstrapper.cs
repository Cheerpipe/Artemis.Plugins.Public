using Artemis.Core;
using Artemis.UI.Shared;

namespace Artemis.Plugins.Modules.Games
{
    public class PluginBootstrapper : IPluginBootstrapper
    {
        public void Disable(Plugin plugin)
        {
        }

        public void Enable(Plugin plugin)
        {
           // plugin.ConfigurationDialog = new PluginConfigurationDialog<GamesModuleConfigDialogViewModel>();
        }
    }
}

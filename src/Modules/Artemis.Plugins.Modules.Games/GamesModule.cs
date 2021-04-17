using Artemis.Core;
using Artemis.Core.Modules;
using SkiaSharp;
using System.Collections.Generic;

namespace Artemis.Plugins.Modules.Games
{
    [PluginFeature(AlwaysEnabled = true)]
    public class GamesModule : ProfileModule
    {
        private PluginSetting<string> _gamesModulePathWildCard;
        private ProcessPathContainsActivationRequirement _processPathContainsActivationRequirement;

        // This is the beginning of your plugin life cycle. Use this instead of a constructor.

        public override void Enable()
        {
            DisplayName = "Games";
            DisplayIcon = "gamepad";
            DefaultPriorityCategory = ModulePriorityCategory.Application;
            _processPathContainsActivationRequirement = new ProcessPathContainsActivationRequirement(_gamesModulePathWildCard.Value);
            ActivationRequirements.Add(_processPathContainsActivationRequirement);

            ModuleTabs = new List<ModuleTab> { new ModuleTab<GamesModuleConfigDialogViewModel>("Configuration") };

        }

        public GamesModule(PluginSettings settings)
        {
            _gamesModulePathWildCard = settings.GetSetting("GamesModulePathWildCard", "juegos");
            _gamesModulePathWildCard.SettingChanged += _gamesModulePathWildCard_SettingChanged;
        }

        private void _gamesModulePathWildCard_SettingChanged(object sender, System.EventArgs e)
        {
            _processPathContainsActivationRequirement.LocationWildCard = _gamesModulePathWildCard.Value;
        }

        // This is the end of your plugin life cycle.
        public override void Disable()
        {
            // Make sure to clean up resources where needed (dispose IDisposables etc.)
        }

        public override void ModuleActivated(bool isOverride)
        {
            // When this gets called your activation requirements have been met and the module will start displaying
        }

        public override void ModuleDeactivated(bool isOverride)
        {
            // When this gets called your activation requirements are no longer met and your module will stop displaying
        }

        public override void Update(double deltaTime)
        {
        }

        public override void Render(double deltaTime, SKCanvas canvas, SKImageInfo canvasInfo)
        {
        }
    }
}
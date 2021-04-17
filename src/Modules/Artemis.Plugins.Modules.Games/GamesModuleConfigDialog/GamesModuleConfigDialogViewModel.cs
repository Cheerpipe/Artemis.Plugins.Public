using Artemis.Core;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Modules;

namespace Artemis.Plugins.Modules.Games
{
    public class GamesModuleConfigDialogViewModel : ModuleViewModel
    {
        private PluginSetting<string> _gamesModulePathWildCard;
        private string _gamesModulePathWildCardSetting;
        public GamesModuleConfigDialogViewModel(GamesModule module, PluginSettings settings) : base(module, "Configuration")
        {
            _gamesModulePathWildCard = settings.GetSetting("GamesModulePathWildCard", @"\games\");
            _gamesModulePathWildCardSetting = _gamesModulePathWildCard.Value;
        }

        public void Save()
        {
            _gamesModulePathWildCard.Value = _gamesModulePathWildCardSetting;
            _gamesModulePathWildCard.Save();

           // RequestClose();
        }

        public string WildCard
        {
            get => _gamesModulePathWildCardSetting;
            set => SetAndNotify(ref _gamesModulePathWildCardSetting, value);
        }

        public void Cancel()
        {
            //RequestClose();
        }
    }
}

using System.Collections.Generic;
using Artemis.Core;
using Artemis.Plugins.LayerBrushes.Hotbar.Services;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Services;

namespace Artemis.Plugins.LayerBrushes.Hotbar.ViewModels
{
    public class PathSetupPropertyEditorViewModel : PropertyInputViewModel<List<PersistentLed>>
    {
        private readonly IDialogService _dialogService;
        public PathSetupPropertyEditorViewModel(LayerProperty<List<PersistentLed>> layerProperty, IProfileEditorService profileEditorService, IDialogService dialogService) : base(layerProperty, profileEditorService)
        {
            _dialogService = dialogService;
        }

        public async void Browse()
        {
            Dictionary<string, object> viewModelVars = new Dictionary<string, object>();
            viewModelVars.Add("sortedLeds", LayerProperty.BaseValue);

            if (await _dialogService.ShowDialog<PathSetupViewModel>(viewModelVars) is List<PersistentLed> sortedLeds)
            {
                LayerProperty.BaseValue = sortedLeds;
                NotifyOfPropertyChange(nameof(LayerProperty));
            }
        }
    }
}

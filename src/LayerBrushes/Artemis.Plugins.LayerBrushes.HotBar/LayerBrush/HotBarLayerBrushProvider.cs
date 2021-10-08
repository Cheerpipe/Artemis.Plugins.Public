using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Hotbar.ViewModels;
using Artemis.UI.Shared.Services;

namespace Artemis.Plugins.LayerBrushes.Hotbar.LayerBrush
{
    public class HotbarLayerBrushProvider : LayerBrushProvider
    {
        private readonly IProfileEditorService _profileEditorService;

        public HotbarLayerBrushProvider(IProfileEditorService profileEditorService)
        {
            _profileEditorService = profileEditorService;
        }
        public override void Enable()
        {
            _profileEditorService.RegisterPropertyInput<PathSetupPropertyEditorViewModel>(Plugin);
            RegisterLayerBrushDescriptor<HotbarLayerBrush>("Hotbar", "Aurora like hotbar layer", "Hotbar.svg");
        }

        public override void Disable()
        {
        }
    }
}
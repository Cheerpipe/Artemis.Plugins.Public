using Artemis.Core.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.Hotbar.LayerBrush
{
    public class HotbarLayerBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<HotbarLayerBrush>("Horbar", "Aurora like hotbar layer", "Hotbar.svg");
        }

        public override void Disable()
        {
        }
    }
}
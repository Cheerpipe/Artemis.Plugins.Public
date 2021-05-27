using Artemis.Core.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.Hotbar.LayerBrush
{
    public class HotbarLayerBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<HotbarLayerBrush>("Horbar", "Artemis Like hotbar layer", "KeyboardVariant");
        }

        public override void Disable()
        {
        }
    }
}
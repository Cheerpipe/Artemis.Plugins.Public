using Artemis.Core.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush
{
    public class ShuffleLayerBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<ShuffleLayerBrush>("Shuffle", "Per key color shuffle", "Palette");
        }

        public override void Disable()
        {
        }
    }
}
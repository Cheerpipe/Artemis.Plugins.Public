using Artemis.Core.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerBrush
{
    public class RipplesLayerBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<RipplesLayerBrush>("Ripples", "Provides ripples effects", "CircleOutline");
        }

        public override void Disable()
        {
        }
    }
}
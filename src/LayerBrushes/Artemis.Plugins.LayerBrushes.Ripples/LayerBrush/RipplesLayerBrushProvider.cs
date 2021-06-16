using Artemis.Core.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerBrush
{
    public class RipplesLayerBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<RipplesLayerBrush>("Ripples", "Provides a highly configurable ripple effect", "Ripples.svg");
        }

        public override void Disable()
        {
        }
    }
}
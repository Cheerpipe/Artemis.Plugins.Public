using Artemis.Core.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.DrippingPaint.LayerBrush
{
    public class DrippingPaintLayerBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<DrippingPaintLayerBrush>("Dripping Paint", "Wall paint dripping layyer effect", "DrippingPaint.svg");
        }

        public override void Disable()
        {
        }
    }
}
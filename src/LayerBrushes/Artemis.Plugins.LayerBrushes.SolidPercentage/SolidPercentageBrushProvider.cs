using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.SolidPercentage.Brushes;

namespace Artemis.Plugins.LayerBrushes.SolidPercentage
{
    public class SolidPercentageBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<SolidPercentageBrush>("Solid Percentage", "Fills the entire layer with a solid color based on a gradient position", "percent");
        }

        public override void Disable()
        {
        }
    }
}
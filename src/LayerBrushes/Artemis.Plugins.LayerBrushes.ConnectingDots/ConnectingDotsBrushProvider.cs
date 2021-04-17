using Artemis.Core.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.ConnectingDots
{
    public class ConnectingDotsBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<ConnectingDotsBrush>("Connecting Dots", "Animated random path circles connected by lines", "ShareVariant");
        }

        public override void Disable()
        {
        }
    }
}
using Artemis.Core.LayerBrushes;

namespace Artemis.Plugins.LayerBrushes.Nexus.LayerBrush
{
    public class NexusLayerBrushProvider : LayerBrushProvider
    {
        public override void Enable()
        {
            RegisterLayerBrushDescriptor<NexusLayerBrush>("Nexus", "Provides a Nexus layer effect", "Nexus.svg");
        }

        public override void Disable()
        {
        }
    }
}
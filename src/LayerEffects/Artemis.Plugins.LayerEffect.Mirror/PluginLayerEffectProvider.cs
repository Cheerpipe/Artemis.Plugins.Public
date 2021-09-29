using Artemis.Core.LayerEffects;

namespace Artemis.Plugins.LayerEffect.Mirror
{
    public class PluginLayerEffectProvider : LayerEffectProvider
    {
        public override void Enable()
        {
            RegisterLayerEffectDescriptor<MirrorPluginLayerEffect>("Mirror", "Provides a horizontal and vertical mirrored layer effect", "Mirror");
        }

        public override void Disable()
        {
        }
    }
}
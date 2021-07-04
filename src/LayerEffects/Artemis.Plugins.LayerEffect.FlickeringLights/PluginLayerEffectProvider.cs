using Artemis.Core.LayerEffects;

namespace Artemis.Plugins.LayerEffect.FlickeringLights
{
    public class PluginLayerEffectProvider : LayerEffectProvider
    {
        public override void Enable()
        {
            RegisterLayerEffectDescriptor<FlickeringLightsPluginLayerEffect>("FlickeringLights", "Provides a game Doom/Half life light flickering", "LightbulbGroup");
        }

        public override void Disable()
        {
        }
    }
}
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Ripples.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerProperties.Presets
{
    public class FireExplosions : ILayerBrushPreset
    {
        private readonly RipplesLayerBrushProperties _properties;

        public FireExplosions(RipplesLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Fire Explosions";
        public string Description => "Fire explosions ripples";
        public string Icon => "Fire";

        public void Apply()
        {
            _properties.ColorMode.SetCurrentValue(ColorType.ColorSet, null);
            ColorGradient gradient = new ColorGradient();
            gradient.Add(new ColorGradientStop(SKColors.Red, 0));
            gradient.Add(new ColorGradientStop(SKColors.Yellow, 1f));
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.Color.SetCurrentValue(SKColors.Transparent, null);
            _properties.RippleFadeAway.SetCurrentValue(RippleFadeOutMode.Medium, null);
            _properties.RippleTrail.SetCurrentValue(true, null);
            _properties.RippleSpawnSpeed.SetCurrentValue(20, null);
            _properties.RippleSpawnAmount.SetCurrentValue(3, null);
            _properties.RippleWidth.SetCurrentValue(40, null);
            _properties.RippleSize.SetCurrentValue(100, null);
            _properties.RippleGrowthSpeed.SetCurrentValue(60, null);
        }
    }
}
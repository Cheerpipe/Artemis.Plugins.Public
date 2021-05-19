using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Ripples.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerProperties.Presets
{
    public class Citrus : ILayerBrushPreset
    {
        private readonly RipplesLayerBrushProperties _properties;

        public Citrus(RipplesLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Citrus";
        public string Description => "Citrus color themed ripples";
        public string Icon => "FruitCitrus";

        public void Apply()
        {
            _properties.ColorMode.SetCurrentValue(ColorType.ColorSet, null);
            ColorGradient gradient = new ColorGradient();
            gradient.Add(new ColorGradientStop(new SKColor(255, 193, 74), 0));
            gradient.Add(new ColorGradientStop(new SKColor(255, 220, 72), 0.33f));
            gradient.Add(new ColorGradientStop(new SKColor(181, 253, 78), 66f));
            gradient.Add(new ColorGradientStop(new SKColor(146, 242, 86), 1f));
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.Color.SetCurrentValue(SKColors.Transparent, null);
            _properties.RippleFadeAway.SetCurrentValue(RippleFadeOutMode.Hard, null);
            _properties.RippleTrail.SetCurrentValue(true, null);
            _properties.RippleSpawnSpeed.SetCurrentValue(20, null);
            _properties.RippleSpawnAmount.SetCurrentValue(2, null);
            _properties.RippleWidth.SetCurrentValue(50, null);
            _properties.RippleSize.SetCurrentValue(100, null);
            _properties.RippleGrowthSpeed.SetCurrentValue(60, null);
        }
    }
}
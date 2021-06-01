using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Ripples.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerProperties.Presets
{
    public class Artemis : ILayerBrushPreset
    {
        private readonly RipplesLayerBrushProperties _properties;

        public Artemis(RipplesLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Artemis";
        public string Description => "Artemis default colors";
        public string Icon => "Water";

        public void Apply()
        {
            _properties.ColorMode.SetCurrentValue(ColorType.ColorSet, null);
            ColorGradient gradient = new ColorGradient
            {
                new (new SKColor(255, 109, 0), 0),
                new (new SKColor(239, 23, 136), 0.33f),
                new (new SKColor(236, 5, 166), 66f),
                new (new SKColor(0, 252, 204), 1f)
            };
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.Color.SetCurrentValue(SKColors.Transparent, null);
            _properties.RippleFadeAway.SetCurrentValue(RippleFadeOutMode.Hard, null);
            _properties.RippleTrail.SetCurrentValue(true, null);
            _properties.RippleSpawnSpeed.SetCurrentValue(200, null);
            _properties.RippleSpawnAmount.SetCurrentValue(2, null);
            _properties.RippleWidth.SetCurrentValue(50, null);
            _properties.RippleSize.SetCurrentValue(200, null);
            _properties.RippleGrowthSpeed.SetCurrentValue(60, null);
        }
    }
}
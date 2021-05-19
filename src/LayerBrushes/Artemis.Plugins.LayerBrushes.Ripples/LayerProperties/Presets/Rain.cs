using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Ripples.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerProperties.Presets
{
    public class Rain : ILayerBrushPreset
    {
        private readonly RipplesLayerBrushProperties _properties;

        public Rain(RipplesLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Rain";
        public string Description => "Winter rain ripples";
        public string Icon => "Water";

        public void Apply()
        {
            _properties.ColorMode.SetCurrentValue(ColorType.ColorSet, null);
            ColorGradient gradient = new ColorGradient();
            gradient.Add(new ColorGradientStop(new SKColor(255, 255, 255), 0));
            gradient.Add(new ColorGradientStop(new SKColor(54, 156, 255), 0.55f));
            gradient.Add(new ColorGradientStop(new SKColor(162, 162, 162), 1));
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.Color.SetCurrentValue(SKColors.Transparent, null);
            _properties.RippleFadeAway.SetCurrentValue(RippleFadeOutMode.Linear, null);
            _properties.RippleTrail.SetCurrentValue(false, null);
            _properties.RippleSpawnSpeed.SetCurrentValue(100, null);
            _properties.RippleSpawnAmount.SetCurrentValue(5, null);
            _properties.RippleWidth.SetCurrentValue(20, null);
            _properties.RippleSize.SetCurrentValue(20, null);
            _properties.RippleGrowthSpeed.SetCurrentValue(30, null);
        }
    }
}
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Ripples.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerProperties.Presets
{
    public class Fall : ILayerBrushPreset
    {
        private readonly RipplesLayerBrushProperties _properties;

        public Fall(RipplesLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Fall";
        public string Description => "Fall rain ripples";
        public string Icon => "LeafMaple";

        public void Apply()
        {
            _properties.ColorMode.SetCurrentValue(ColorType.ColorSet, null);
            ColorGradient gradient = new ColorGradient();
            gradient.Add(new ColorGradientStop(new SKColor(232, 148, 135), 0.0f));
            gradient.Add(new ColorGradientStop(new SKColor(245, 238, 220), 0.25f));
            gradient.Add(new ColorGradientStop(new SKColor(154, 192, 188), 0.5f));
            gradient.Add(new ColorGradientStop(new SKColor(146, 119, 130), 0.75f));
            gradient.Add(new ColorGradientStop(new SKColor(46, 66, 72), 1f));
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
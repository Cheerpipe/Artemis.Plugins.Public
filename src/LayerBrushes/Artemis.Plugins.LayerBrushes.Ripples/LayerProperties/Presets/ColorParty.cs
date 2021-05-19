using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Ripples.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerProperties.Presets
{
    public class ColorParty : ILayerBrushPreset
    {
        private readonly RipplesLayerBrushProperties _properties;

        public ColorParty(RipplesLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Color Party";
        public string Description => "Unicorn color party ripples";
        public string Icon => "Looks";

        public void Apply()
        {
            _properties.ColorMode.SetCurrentValue(ColorType.ColorSet, null);
            _properties.Colors.SetCurrentValue(ColorGradient.GetUnicornBarf(), null);
            _properties.Color.SetCurrentValue(SKColors.Transparent, null);
            _properties.RippleFadeAway.SetCurrentValue(RippleFadeOutMode.Hard, null);
            _properties.RippleTrail.SetCurrentValue(true, null);
            _properties.RippleSpawnSpeed.SetCurrentValue(20, null);
            _properties.RippleSpawnAmount.SetCurrentValue(2, null);
            _properties.RippleWidth.SetCurrentValue(50, null);
            _properties.RippleSize.SetCurrentValue(200, null);
            _properties.RippleGrowthSpeed.SetCurrentValue(60, null);
        }
    }
}
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerProperties.Presets
{
    public class WhiteNoise : ILayerBrushPreset
    {
        private readonly ShuffleLayerBrushProperties _properties;

        public WhiteNoise(ShuffleLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "White noise";
        public string Description => "White noise effect";
        public string Icon => "Palette";

        public void Apply()
        {
            ColorGradient gradient = new ColorGradient
            {
                new (new SKColor(255, 255, 255), 0),
                new (new SKColor(50, 50, 50), 1f)
            };
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.SmoothColorChange.SetCurrentValue(false, null);
            _properties.ChangeSpeed.SetCurrentValue(new FloatRange(500, 1000), null);
        }
    }
}
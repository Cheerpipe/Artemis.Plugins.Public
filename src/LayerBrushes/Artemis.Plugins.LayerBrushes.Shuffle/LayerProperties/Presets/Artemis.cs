using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerProperties.Presets
{
    public class Artemis : ILayerBrushPreset
    {
        private readonly ShuffleLayerBrushProperties _properties;

        public Artemis(ShuffleLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Artemis";
        public string Description => "Artemis color set";
        public string Icon => "Palette";

        public void Apply()
        {
            ColorGradient gradient = new ColorGradient
            {
                new (new SKColor(255, 109, 0), 0),
                new (new SKColor(239, 23, 136), 0.33f),
                new (new SKColor(236, 5, 166), 66f),
                new (new SKColor(0, 252, 204), 1f)
            };
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.SmoothColorChange.SetCurrentValue(true, null);
            _properties.ChangeSpeed.SetCurrentValue(new FloatRange(100, 200), null);
        }
    }
}
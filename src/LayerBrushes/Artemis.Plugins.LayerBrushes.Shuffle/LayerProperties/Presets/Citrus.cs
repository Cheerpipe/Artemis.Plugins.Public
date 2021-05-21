using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerProperties.Presets
{
    public class Citrus : ILayerBrushPreset
    {
        private readonly ShuffleLayerBrushProperties _properties;

        public Citrus(ShuffleLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Citrus";
        public string Description => "Citrus color set";
        public string Icon => "Palette";

        public void Apply()
        {
            ColorGradient gradient = new ColorGradient();
            gradient.Add(new ColorGradientStop(new SKColor(255, 193, 74), 0));
            gradient.Add(new ColorGradientStop(new SKColor(255, 220, 72), 0.33f));
            gradient.Add(new ColorGradientStop(new SKColor(181, 253, 78), 66f));
            gradient.Add(new ColorGradientStop(new SKColor(146, 242, 86), 1f));
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.SmoothColorChange.SetCurrentValue(true, null);
            _properties.ChangeSpeed.SetCurrentValue(new FloatRange(100, 200), null);
        }
    }
}
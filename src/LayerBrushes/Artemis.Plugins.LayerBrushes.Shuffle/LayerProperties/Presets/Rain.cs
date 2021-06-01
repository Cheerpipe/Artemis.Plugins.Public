using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerProperties.Presets
{
    public class Rain : ILayerBrushPreset
    {
        private readonly ShuffleLayerBrushProperties _properties;

        public Rain(ShuffleLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Rain";
        public string Description => "Rain color set";
        public string Icon => "Palette";

        public void Apply()
        {
            ColorGradient gradient = new ColorGradient
            {
                new (new SKColor(255, 255, 255), 0),
                new (new SKColor(54, 156, 255), 0.55f),
                new (new SKColor(162, 162, 162), 1)
            };
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.SmoothColorChange.SetCurrentValue(true, null);
            _properties.ChangeSpeed.SetCurrentValue(new FloatRange(200, 400), null);
        }
    }
}
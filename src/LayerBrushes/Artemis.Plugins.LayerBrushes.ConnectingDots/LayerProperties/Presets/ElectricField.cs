using Artemis.Core;
using Artemis.Core.LayerBrushes;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.ConnectingDots.LayerProperties.Presets
{
    public class ElectricField : ILayerBrushPreset
    {
        private readonly ConnectingDotsBrushProperties _properties;

        public ElectricField(ConnectingDotsBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Electric Field";
        public string Description => "Dots connected by lines based on their distance";
        public string Icon => "ShareVariant";

        public void Apply()
        {
            _properties.Background.SetCurrentValue(SKColors.Black, null);
            _properties.Connections.SetCurrentValue(new SKColor(0, 203, 238), null);
            _properties.DotsColorType.SetCurrentValue(ConnectingDotsBrushProperties.ColorMappingType.Gradient, null);
            ColorGradient gradient = new ColorGradient
            {
                new (new SKColor(0, 0, 255), 0),
                new (new SKColor(255, 0, 255), 0.33f),
                new (new SKColor(125, 0, 255), 0.66f),
                new (new SKColor(0, 0, 255), 1)
            };
            _properties.Colors.SetCurrentValue(gradient, null);
            _properties.ColorChangeSpeed.SetCurrentValue(20, null);
            _properties.Radius.SetCurrentValue(20, null);
            _properties.ConnectDistance.SetCurrentValue(new FloatRange(50, 200), null);
            _properties.ConnectionWidth.SetCurrentValue(15, null);
            _properties.DotsMovementSpeed.SetCurrentValue(80, null);
            _properties.Dots.SetCurrentValue(15, null);
        }
    }
}
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.ConnectingDots.LayerProperties.Presets
{
    public class BigBouncingBubbles : ILayerBrushPreset
    {
        private readonly ConnectingDotsBrushProperties _properties;

        public BigBouncingBubbles(ConnectingDotsBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Big Bouncing Bubbles";
        public string Description => "Colorful big bouncing bubbles";
        public string Icon => "ChartBubble";

        public void Apply()
        {
            _properties.Background.SetCurrentValue(SKColors.Black, null);
            _properties.Connections.SetCurrentValue(SKColors.Transparent, null);
            _properties.DotsColorType.SetCurrentValue(ConnectingDotsBrushProperties.ColorMappingType.Gradient, null);
            _properties.Colors.SetCurrentValue(ColorGradient.GetUnicornBarf(), null);
            _properties.ColorChangeSpeed.SetCurrentValue(20, null);
            _properties.Radius.SetCurrentValue(50, null);
            _properties.ConnectDistance.SetCurrentValue(new FloatRange(0, 0), null);
            _properties.ConnectionWidth.SetCurrentValue(0, null);
            _properties.DotsMovementSpeed.SetCurrentValue(80, null);
            _properties.Dots.SetCurrentValue(10, null);
        }
    }
}
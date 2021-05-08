using Artemis.Core;
using Artemis.Core.LayerBrushes;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.ConnectingDots.LayerProperties.Presets
{
    public class DancingFireLines : ILayerBrushPreset
    {
        private readonly ConnectingDotsBrushProperties _properties;
        private readonly Plugin _plugin;

        public DancingFireLines(ConnectingDotsBrush brush, Plugin plugin)
        {
            _properties = brush.Properties;
            _plugin = plugin;
        }

        public string Name => "Dancing fire lines";
        public string Description => "Dancing fire lines.";
        public string Icon => _plugin.ResolveRelativePath("ConnectedLines.svg");

        public void Apply()
        {
            _properties.Background.SetCurrentValue(SKColors.Black, null);
            _properties.Connections.SetCurrentValue(SKColors.Red, null);
            _properties.Color.SetCurrentValue(SKColors.Red, null);
            _properties.DotsColorType.SetCurrentValue(ConnectingDotsBrushProperties.ColorMappingType.Simple, null);
            _properties.ColorChangeSpeed.SetCurrentValue(0, null);
            _properties.Radius.SetCurrentValue(5, null);
            _properties.ConnectDistance.SetCurrentValue(new FloatRange(0, 600), null);
            _properties.ConnectionWidth.SetCurrentValue(10, null);
            _properties.DotsMovementSpeed.SetCurrentValue(50, null);
            _properties.Dots.SetCurrentValue(6, null);
        }
    }
}
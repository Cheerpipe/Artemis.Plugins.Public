using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerProperties.Presets
{
    public class Christmas : ILayerBrushPreset
    {
        private readonly ShuffleLayerBrushProperties _properties;

        public Christmas(ShuffleLayerBrush brush)
        {
            _properties = brush.Properties;
        }

        public string Name => "Christmas";
        public string Description => "Christmas color set";
        public string Icon => "Palette";

        public void Apply()
        {
            _properties.Colors.SetCurrentValue(ColorGradient.GetUnicornBarf(), null);
            _properties.SmoothColorChange.SetCurrentValue(false, null);
            _properties.ChangeSpeed.SetCurrentValue(new FloatRange(100, 100), null);
        }
    }
}
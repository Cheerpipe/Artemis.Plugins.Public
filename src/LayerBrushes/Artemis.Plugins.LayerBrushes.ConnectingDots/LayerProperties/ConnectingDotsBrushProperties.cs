using Artemis.Core;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.ConnectingDots
{

    public class ConnectingDotsBrushProperties : LayerPropertyGroup
    {
        [PropertyDescription(Description = "Color of the background")]
        public SKColorLayerProperty Background { get; set; }

        [PropertyDescription(Description = "Colors of the connections between the dots")]
        public SKColorLayerProperty Connections { get; set; }
        [PropertyDescription(Name = "Dots color mapping type")]
        public EnumLayerProperty<ColorMappingType> DotsColorType { get; set; }

        [PropertyDescription(Description = "Color gradient used to colorize dots")]
        public ColorGradientLayerProperty Colors { get; set; }

        [PropertyDescription(Description = "Color used to colorize dots")]
        public SKColorLayerProperty Color { get; set; }

        [PropertyDescription(Description = "Color change speed", MinInputValue = 0)]
        public FloatLayerProperty ColorChangeSpeed { get; set; }

        [PropertyDescription(Description = "Radius of the dots", MinInputValue = 0)]
        public FloatLayerProperty Radius { get; set; }

        [PropertyDescription(Description = "Connect distance", MinInputValue = 0)]
        public FloatRangeLayerProperty ConnectDistance { get; set; }

        [PropertyDescription(Description = "Width of the connection", MinInputValue = 0)]
        public FloatLayerProperty ConnectionWidth { get; set; }

        [PropertyDescription(Description = "Dots movement speed", MinInputValue = 0)]
        public FloatLayerProperty DotsMovementSpeed { get; set; }

        [PropertyDescription(Description = "Dots count", MinInputValue = 0)]
        public IntLayerProperty Dots { get; set; }

        protected override void PopulateDefaults()
        {
            Background.DefaultValue = SKColor.Parse("#FF000000");
            Color.DefaultValue = SKColor.Parse("#FF0000FF");
            Connections.DefaultValue = SKColor.Parse("#FF00FFFF");
            Radius.DefaultValue = 20;
            ConnectDistance.DefaultValue = new FloatRange(0, 100);
            ConnectionWidth.DefaultValue = 10;
            DotsMovementSpeed.DefaultValue = 100;
            ColorChangeSpeed.DefaultValue = 25;
            Dots.DefaultValue = 10;
            DotsColorType.DefaultValue = ColorMappingType.Simple;
            Colors.DefaultValue = ColorGradient.GetUnicornBarf();
        }

        protected override void EnableProperties()
        {
            Colors.IsVisibleWhen(DotsColorType, c => c.BaseValue == ColorMappingType.Gradient);
            ColorChangeSpeed.IsVisibleWhen(DotsColorType, c => c.BaseValue == ColorMappingType.Gradient);
            Color.IsVisibleWhen(DotsColorType, c => c.BaseValue == ColorMappingType.Simple);
        }
        protected override void DisableProperties()
        {
        }

        public enum ColorMappingType
        {
            Simple,
            Gradient
        }

    }
}
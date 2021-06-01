using Artemis.Core;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Hotbar.LayerProperties
{
    public class HotbarLayerBrushProperties : LayerPropertyGroup
    {
        public EnumLayerProperty<KeyColorType> ColorMode { get; set; }
        [PropertyDescription(Description = "Color used to highlight the active key")]
        public SKColorLayerProperty ActiveKeyColor { get; set; }
        [PropertyDescription(Description = "Color used to highlight the active key")]
        public ColorGradientLayerProperty ActiveKeyGradient { get; set; }

        [PropertyDescription(Description = "Paint background")]
        public BoolLayerProperty PaintBackground { get; set; }

        [PropertyDescription(Description = "Color used to paint a layer background")]
        public SKColorLayerProperty BackgroundColor { get; set; }

        [PropertyDescription(Name = "LED order", Description = "The order in which to reveal LEDs")]
        public EnumLayerProperty<LedOrder> LedOrder { get; set; }

        [PropertyDescription(Description = "Handle scroll overflow as a circular led array")]
        public BoolLayerProperty LoopOnScrollOverflow { get; set; }
        [PropertyDescription(Description = "Allows the scroll to activate the hotbar even if there is no active key. Useful if you use hotbar with devices without keys")]
        public BoolLayerProperty ScrollActivation { get; set; }

        protected override void PopulateDefaults()
        {
            ActiveKeyColor.DefaultValue = SKColors.Red;
            ActiveKeyGradient.DefaultValue = ColorGradient.GetUnicornBarf();
            PaintBackground.DefaultValue = true;
            BackgroundColor.DefaultValue = SKColors.Black;
        }

        protected override void EnableProperties()
        {
            ActiveKeyGradient.IsVisibleWhen(ColorMode, c => c.CurrentValue == KeyColorType.Gradient);
            ActiveKeyColor.IsVisibleWhen(ColorMode, c => c.CurrentValue == KeyColorType.Solid);
        }

        protected override void DisableProperties()
        {
        }
    }

    public enum LedOrder
    {
        LedId,
        Vertical,
        Horizontal,
        VerticalReversed,
        HorizontalReversed
    }

    public enum KeyColorType
    {
        Solid,
        Gradient
    }

}

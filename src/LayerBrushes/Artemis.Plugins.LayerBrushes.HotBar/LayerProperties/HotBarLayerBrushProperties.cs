using System.Collections.Generic;
using Artemis.Core;
using Artemis.Plugins.LayerBrushes.Hotbar.Services;
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

        [PropertyDescription(Description = "This feature allows you to sort layer leds as you want to create a custom path", DisableKeyframes = true)]
        public LayerProperty<List<PersistentLed>> LedSortMap { get; set; }

        [PropertyDescription(Description = "Keys will act as a self on/off toggle")]
        public BoolLayerProperty KeyToggle { get; set; }

        [PropertyDescription(Description = "Handle scroll overflow as a circular led array")]
        public BoolLayerProperty LoopOnScrollOverflow { get; set; }

        [PropertyDescription(Description = "Allows the scroll to change active key light")]
        public BoolLayerProperty UseScroll { get; set; }

        [PropertyDescription(Description = "Allows the scroll to activate the hotbar even if there is no active key. Useful if you use hotbar with devices without keys")]
        public BoolLayerProperty ScrollActivation { get; set; }

        protected override void PopulateDefaults()
        {
            ActiveKeyColor.DefaultValue = SKColors.Red;
            ActiveKeyGradient.DefaultValue = ColorGradient.GetUnicornBarf();
            PaintBackground.DefaultValue = true;
            BackgroundColor.DefaultValue = SKColors.Black;
            UseScroll.DefaultValue = false;
            ScrollActivation.DefaultValue = false;
            KeyToggle.DefaultValue = false;
        }

        protected override void EnableProperties()
        {
            ActiveKeyGradient.IsVisibleWhen(ColorMode, c => c.CurrentValue == KeyColorType.Gradient);
            ActiveKeyColor.IsVisibleWhen(ColorMode, c => c.CurrentValue == KeyColorType.Solid);
            LedSortMap.IsVisibleWhen(LedOrder, c => c.CurrentValue == Hotbar.LayerProperties.LedOrder.Custom);
            ScrollActivation.IsVisibleWhen(UseScroll, c => c.CurrentValue == true);
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
        HorizontalReversed,
        Custom
    }

    public enum KeyColorType
    {
        Solid,
        Gradient
    }
}

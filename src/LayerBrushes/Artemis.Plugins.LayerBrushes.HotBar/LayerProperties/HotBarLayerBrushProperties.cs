using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            LedSortMap.IsVisibleWhen(LedOrder, c => c.CurrentValue == Hotbar.LayerProperties.LedOrder.Custom);
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

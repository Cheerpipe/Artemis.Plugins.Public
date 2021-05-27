using Artemis.Core;
using SkiaSharp;
using System.ComponentModel;

namespace Artemis.Plugins.LayerBrushes.Hotbar.LayerProperties
{
    public class HotbarLayerBrushProperties : LayerPropertyGroup
    {
        [PropertyDescription(Description = "Color used to highlight the active key")]
        public SKColorLayerProperty ActiveKeyColor { get; set; }

        [PropertyDescription(Description = "Paint background")]
        public BoolLayerProperty PaintBackground { get; set; }

        [PropertyDescription(Description = "Color used to paint a layer background")]
        public SKColorLayerProperty BackgroundColor { get; set; }

        [PropertyDescription(Name = "LED order", Description = "The order in which to reveal LEDs")]
        public EnumLayerProperty<LedOrder> LedOrder { get; set; }

        protected override void PopulateDefaults()
        {
            ActiveKeyColor.DefaultValue = SKColors.Red;
            PaintBackground.DefaultValue = true;
            BackgroundColor.DefaultValue = SKColors.Black;
        }
        
        protected override void EnableProperties()
        {
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

}

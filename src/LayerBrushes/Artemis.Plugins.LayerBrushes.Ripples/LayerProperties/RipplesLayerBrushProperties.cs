using Artemis.Core;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerProperties
{
    public class RipplesLayerBrushProperties : LayerPropertyGroup
    {

        public EnumLayerProperty<ColorType> ColorMode { get; set; }
        [PropertyDescription(Description = "The gradient of the circle or the colors to cycle through in the echo.")]
        public ColorGradientLayerProperty Colors { get; set; }
        public SKColorLayerProperty Color { get; set; }
        [PropertyDescription(Description = "Fade away ripple effect mode.")]
        public EnumLayerProperty<RippleFadeOutMode> RippleFadeAway { get; set; }
        [PropertyDescription(Description = "Show a soft trail behind the ripple.")]
        public BoolLayerProperty RippleTrail { get; set; }
        [PropertyDescription(Description = "Ripple spawn speed.", MinInputValue = 1)]
        public FloatLayerProperty RippleSpawnSpeed { get; set; }
        [PropertyDescription(Description = "Ripple spawn amount.", MinInputValue = 1, MaxInputValue = 100)]
        public IntLayerProperty RippleSpawnAmount { get; set; }
        [PropertyDescription(Description = "Width of the ripple.")]
        public FloatLayerProperty RippleWidth { get; set; }
        [PropertyDescription(Description = "Size of the expand area of the ripple.")]
        public FloatLayerProperty RippleSize { get; set; }
        [PropertyDescription(Description = "Growth speed of the ripple.")]
        public FloatLayerProperty RippleGrowthSpeed { get; set; }


        protected override void PopulateDefaults()
        {
            ColorMode.DefaultValue = ColorType.ColorSet;
            Colors.DefaultValue = ColorGradient.GetUnicornBarf();
            Color.DefaultValue = new SKColor(255, 0, 0);
            RippleFadeAway.DefaultValue = RippleFadeOutMode.Linear;
            RippleTrail.DefaultValue = true;
            RippleSpawnSpeed.DefaultValue = 50; // 1 Second
            RippleSpawnAmount.DefaultValue = 1;
            RippleWidth.DefaultValue = 40;
            RippleSize.DefaultValue = 200;
            RippleGrowthSpeed.DefaultValue = 300;
        }

        protected override void EnableProperties()
        {
            Colors.IsVisibleWhen(ColorMode, c => c.CurrentValue == ColorType.Gradient || c.CurrentValue == ColorType.ColorPathChange || c.CurrentValue == ColorType.ColorSet);
            Color.IsVisibleWhen(ColorMode, c => c.CurrentValue == ColorType.Solid);
        }

        protected override void DisableProperties()
        {
        }
    }

    public enum ColorType
    {
        Random,
        Solid,
        Gradient,
        ColorPathChange,
        ColorSet
    }

    public enum RippleFadeOutMode
    {
        None = -1,
        Linear = Easings.Functions.Linear,
        Soft = Easings.Functions.CircularEaseIn,
        Medium = Easings.Functions.SineEaseOut,
        Hard = Easings.Functions.ExponentialEaseOut
    }
}
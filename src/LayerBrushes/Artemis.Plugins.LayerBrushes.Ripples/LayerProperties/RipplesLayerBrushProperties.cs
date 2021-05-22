using Artemis.Core;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerProperties
{
    public class RipplesLayerBrushProperties : LayerPropertyGroup
    {

        public EnumLayerProperty<ColorType> ColorMode { get; set; }

        [PropertyDescription(Description = "The gradient tha defines the colors that are being used for draw the ripples.")]
        public ColorGradientLayerProperty Colors { get; set; }

        [PropertyDescription(Description = "Color used to create ripples.")]
        public SKColorLayerProperty Color { get; set; }

        [PropertyDescription(Description = "Location of the layer where new ripples will be spawned.")]
        public EnumLayerProperty<RippleSpawnLocation> RippleSpawnLocation { get; set; }

        [PropertyDescription(Description = "Set a percentage relative X,Y point to spawn ripples. Useful to bind spawn point using DataModels.", InputAffix = "%", MinInputValue = 0f, MaxInputValue = 100f)]
        public FloatRangeLayerProperty RippleSpawnPoint { get; set; }

        [PropertyDescription(Description = "Fade away ripple effect mode.")]
        public EnumLayerProperty<RippleFadeOutMode> RippleFadeAway { get; set; }

        [PropertyDescription(Description = "Show a soft trail behind the ripple.")]
        public BoolLayerProperty RippleTrail { get; set; }

        [PropertyDescription(Description = "Ripple spawn speed. 100 is equal to one second", MinInputValue = 1)]
        public FloatLayerProperty RippleSpawnSpeed { get; set; }
        [PropertyDescription(Description = "Ripple spawn amount.")]
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
            RippleSpawnLocation.DefaultValue = Ripples.LayerProperties.RippleSpawnLocation.Random;
            RippleSpawnPoint.DefaultValue= new FloatRange(50f, 50f);
            RippleTrail.DefaultValue = true;
            RippleSpawnSpeed.DefaultValue = 500; // 1 Second
            RippleSpawnAmount.DefaultValue = 1;
            RippleWidth.DefaultValue = 40;
            RippleSize.DefaultValue = 200;
            RippleGrowthSpeed.DefaultValue = 300;
        }

        protected override void EnableProperties()
        {
            Colors.IsVisibleWhen(ColorMode, c => c.CurrentValue == ColorType.Gradient || c.CurrentValue == ColorType.ColorPathChange || c.CurrentValue == ColorType.ColorSet);
            Color.IsVisibleWhen(ColorMode, c => c.CurrentValue == ColorType.Solid);
            RippleSpawnPoint.IsVisibleWhen(RippleSpawnLocation, c => c.CurrentValue == Ripples.LayerProperties.RippleSpawnLocation.RelativePoint);

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

    public enum RippleSpawnLocation
    {
        Random,
        RelativePoint,
        TopLeft,
        TopCentre,
        TopRight,
        MiddleLeft,
        Centre,
        MiddleRight,
        BottomLeft,
        BottomCentre,
        BottomRight
    }
}
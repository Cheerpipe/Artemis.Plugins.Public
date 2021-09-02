using Artemis.Core;

namespace Artemis.Plugins.LayerBrushes.DrippingPaint.LayerProperties
{
    public class DrippingPaintLayerBrushProperties : LayerPropertyGroup
    {
        public EnumLayerProperty<ColorType> ColorMode { get; set; }
        [PropertyDescription(Description = "The gradient tha defines the colors that are being used for draw the ripples")]
        public ColorGradientLayerProperty Colors { get; set; }
        [PropertyDescription(Description = "Set how new pain waves will be dripped")]
        public EnumLayerProperty<DrippingTrigger> DrippingTrigger { get; set; }
        [PropertyDescription(Description = "Set how new paint waves will be dripped", MinInputValue = 1, MaxInputValue = 100)]
        public FloatLayerProperty Threshold { get; set; }
        [PropertyDescription(Description = "Set the delay time between waves")]
        public FloatLayerProperty Delay { get; set; }
        [PropertyDescription(Description = "Set the delay time between waves", MinInputValue = 1)]
        public IntLayerProperty DripCount { get; set; }
        [PropertyDescription(Description = "This option works as a global speed modifier. Usefull to use with databindings", InputAffix = "%", MinInputValue = 1)]
        public FloatLayerProperty InitialSpeed { get; set; }

        [PropertyDescription(Description = "This option determine how long the drop trail will be drawn", MinInputValue = 1)]
        public FloatRangeLayerProperty DropGravity { get; set; }

        protected override void PopulateDefaults()
        {
            ColorMode.DefaultValue = ColorType.Random;
            Colors.DefaultValue = ColorGradient.GetUnicornBarf();

            InitialSpeed.DefaultValue = 100f;
            DropGravity.DefaultValue = new FloatRange(10f, 100f);

            DrippingTrigger.DefaultValue = DrippingPaint.LayerProperties.DrippingTrigger.Threshold;

            Threshold.DefaultValue = 80f;
            Delay.DefaultValue = 1000f;

            DripCount.DefaultValue = 10;
        }

        protected override void EnableProperties()
        {
            Colors.IsVisibleWhen(ColorMode, c => c.CurrentValue == ColorType.ColorSet);
            Threshold.IsVisibleWhen(DrippingTrigger, c => c.CurrentValue == DrippingPaint.LayerProperties.DrippingTrigger.Threshold);
            Delay.IsVisibleWhen(DrippingTrigger, c => c.CurrentValue == DrippingPaint.LayerProperties.DrippingTrigger.Delay);
        }

        protected override void DisableProperties()
        {
        }
    }

    public enum DrippingTrigger
    {
        Delay,
        Threshold
    }
    public enum ColorType
    {
        Random,
        ColorSet
    }
}
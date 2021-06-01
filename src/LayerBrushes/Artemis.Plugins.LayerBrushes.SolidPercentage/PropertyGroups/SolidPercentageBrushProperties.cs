using Artemis.Core;

namespace Artemis.Plugins.LayerBrushes.SolidPercentage.PropertyGroups
{
    public class SolidPercentageBrushProperties : LayerPropertyGroup
    {

        [PropertyDescription(Description = "Color gradient")]
        public ColorGradientLayerProperty Colors { get; set; }

        [PropertyDescription(Description = "Mini and max range value")]
        public FloatRangeLayerProperty MinAndMax { get; set; }

        [PropertyDescription(Description = "Current value used to pick a color from the gradient based on the range value. THIS VALUE MUST HAVE A DATABIND")]
        public FloatLayerProperty CurrentValue { get; set; }

        [PropertyDescription(Description = "Automatically expand the lower limit of the range. New min value won't be saved")]
        public BoolLayerProperty AutoExpandMinValue { get; set; }

        [PropertyDescription(Description = "Automatically expand the upper limit of the range. New max value won't be saved")]
        public BoolLayerProperty AutoExpandMaxValue { get; set; }

        protected override void PopulateDefaults()
        {
            Colors.DefaultValue = ColorGradient.GetUnicornBarf();
            MinAndMax.DefaultValue = new FloatRange(0, 100);
            AutoExpandMinValue.DefaultValue = false;
            AutoExpandMaxValue.DefaultValue = false;
        }

        protected override void EnableProperties()
        {
        }

        protected override void DisableProperties()
        {
        }
    }
}
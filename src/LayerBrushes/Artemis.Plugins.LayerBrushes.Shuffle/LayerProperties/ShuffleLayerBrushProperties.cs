using Artemis.Core;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerProperties
{
    public class ShuffleLayerBrushProperties : LayerPropertyGroup
    {
        [PropertyDescription(Description = "The gradient used to create a color set for the shuffle effect.")]
        public ColorGradientLayerProperty Colors { get; set; }

      
        [PropertyDescription(Description = "Time between color changes. 100 is equal to one second", MinInputValue = 1)]
        public FloatRangeLayerProperty ChangeSpeed { get; set; }

        [PropertyDescription(Description = "Smooth color change")]
        public BoolLayerProperty SmoothColorChange { get; set; }

        protected override void PopulateDefaults()
        {

            Colors.DefaultValue = ColorGradient.GetUnicornBarf();
            ChangeSpeed.DefaultValue = new FloatRange(100, 200);
            SmoothColorChange.DefaultValue = true;
        }

        protected override void EnableProperties()
        {
        }

        protected override void DisableProperties()
        {
        }
    }
}
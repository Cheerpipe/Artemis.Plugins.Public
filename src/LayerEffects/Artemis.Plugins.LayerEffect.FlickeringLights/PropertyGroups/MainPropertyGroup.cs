using System.Diagnostics.CodeAnalysis;
using Artemis.Core;

namespace Artemis.Plugins.LayerEffect.FlickeringLights.PropertyGroups
{
    public class MainPropertyGroup : LayerEffectPropertyGroup
    {
        [PropertyDescription(Description = "Set the flickering pattern used to turn lights on and off")]
        public LayerProperty<string> FlickeringPattern { get; set; }

        [PropertyDescription(Description = "Time in seconds to play a complete light sequence and start again", InputAffix = "Sec", MinInputValue = 0.1f, MaxInputValue = float.MaxValue)]
        public FloatLayerProperty LoopTime { get; set; }

        protected override void PopulateDefaults()
        {
            FlickeringPattern.DefaultValue = "mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa";
            LoopTime.DefaultValue = 5f;
        }

        protected override void EnableProperties() { }

        protected override void DisableProperties() { }
    }
}
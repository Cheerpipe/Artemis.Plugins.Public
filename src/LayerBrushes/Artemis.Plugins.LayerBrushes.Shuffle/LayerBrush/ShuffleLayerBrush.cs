using System;
using System.Collections.Generic;
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Shuffle.LayerProperties;
using Artemis.Plugins.LayerBrushes.Shuffle.LayerProperties.Presets;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush
{
    public class ShuffleLayerBrush : PerLedLayerBrush<ShuffleLayerBrushProperties>
    {
        #region  Variables 

        private readonly Dictionary<ArtemisLed, ShuffledLed> _layerLeds;
        private readonly Profiler _profiler;

        #endregion

        #region Constructor

        public ShuffleLayerBrush(Plugin plugin)
        {
            _layerLeds = new Dictionary<ArtemisLed, ShuffledLed>();
            _profiler = plugin.GetProfiler("ShuffleLayerBrush");
          
        }

        #endregion

        #region Pluging lifecicle methods

        public override void EnableLayerBrush()
        {
            Layer.RenderPropertiesUpdated += Layer_RenderPropertiesUpdated;
        }

        public override void DisableLayerBrush()
        {
            Layer.RenderPropertiesUpdated -= Layer_RenderPropertiesUpdated;
        }

        private void Layer_RenderPropertiesUpdated(object sender, EventArgs e)
        {
            lock (_layerLeds)
            {
                _layerLeds.Clear();
            }
        }

        public override List<ILayerBrushPreset> Presets => new()
        {
            new LayerProperties.Presets.Artemis(this),
            new Christmas(this),
            new Citrus(this),
            new Rain(this),
            new WhiteNoise(this),
        };


        #endregion

        #region Pluging behavior methods

        public override void Update(double deltaTime)
        {

            // Ignore negative updates coming from the editor, no negativity here!
            if (deltaTime <= 0)
                return;

            _profiler.StartMeasurement("Update");

            lock (_layerLeds)
            {
                foreach (ArtemisLed led in Layer.Leds)
                {
                    if (_layerLeds.TryGetValue(led, out ShuffledLed shuffleLayerBrush))
                        //return shuffleLayerBrush.GetCurrentColor();
                        shuffleLayerBrush.Advance((float)deltaTime);
                    else
                        _layerLeds[led] = new ShuffledLed(this);
                }
            }
            _profiler.StopMeasurement("Update");
        }

        public override SKColor GetColor(ArtemisLed led, SKPoint renderPoint)
        {
            lock (_layerLeds)
            {
                return _layerLeds.TryGetValue(led, out ShuffledLed shuffleLayerBrush) ? shuffleLayerBrush.GetCurrentColor() : SKColors.Transparent;
            }
        }

        #endregion
    }
}
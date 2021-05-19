using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.Ripples.LayerProperties;
using Artemis.Plugins.LayerBrushes.Ripples.LayerProperties.Presets;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerBrush
{
    public class RipplesLayerBrush : LayerBrush<RipplesLayerBrushProperties>
    {
        private readonly List<Ripple> _ripples;
        public Random Rand { get; set; }

        public RipplesLayerBrush()
        {
            _ripples = new List<Ripple>();
        }

        public override List<ILayerBrushPreset> Presets => new()
        {
            new LayerProperties.Presets.Artemis(this),
            new Citrus(this),
            new ColorParty(this),
            new FireExplosions(this),
            new Rain(this)
        };

        public override void EnableLayerBrush()
        {
            Rand = new Random(Layer.EntityId.GetHashCode());
        }

        public override void DisableLayerBrush()
        {

        }

        private float _lastSpawnTime = 0;
        public override void Update(double deltaTime)
        {

            // Ignore negative updates coming from the editor, no negativity here!
            if (deltaTime <= 0)
                return;

            // Simple linear time spawn
            _lastSpawnTime += (float)deltaTime;
            if (_lastSpawnTime > 10f / Properties.RippleSpawnSpeed.CurrentValue)
            {
                for (int i = 0; i < Properties.RippleSpawnAmount.CurrentValue; i++)
                {
                    SKPoint spawnPoint = new SKPoint(
                    Rand.Next(1, Layer.Bounds.Width),
                   Rand.Next(1, Layer.Bounds.Height)
                    );
                    SpawnEffect(spawnPoint);
                }
                _lastSpawnTime = 0;
            }


            lock (_ripples)
            {
                _ripples.RemoveAll(w => w.Finished);
                foreach (Ripple keypressWave in _ripples)
                    keypressWave.Update(deltaTime);
            }
        }

        public override void Render(SKCanvas canvas, SKRect bounds, SKPaint paint)
        {
            lock (_ripples)
            {
                foreach (Ripple effect in _ripples)
                    effect.Render(canvas);
            }
        }

        private void SpawnEffect(SKPoint relativeLedPosition)
        {
            lock (_ripples)
            {
                _ripples.Add(new Ripple(this, relativeLedPosition));
            }
        }
    }
}
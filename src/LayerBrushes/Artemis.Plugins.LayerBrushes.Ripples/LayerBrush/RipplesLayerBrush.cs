using System;
using System.Collections.Generic;
using System.Linq;
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
            new Rain(this),
            new Fall(this)
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
            if (_lastSpawnTime > 100f / Properties.RippleSpawnSpeed.CurrentValue)
            {
                for (int i = 0; i < Properties.RippleSpawnAmount.CurrentValue; i++)
                {
                    int x = 1;
                    int y = 1;

                    // Override default spawn location
                    switch (Properties.RippleSpawnLocation.CurrentValue)
                    {
                        case RippleSpawnLocation.Random:
                            x = Rand.Next(1, Layer.Bounds.Width);
                            y = Rand.Next(1, Layer.Bounds.Height);
                            break;
                        case RippleSpawnLocation.RelativePoint:
                            x = Math.Clamp((int)(Layer.Bounds.Width * Properties.RippleSpawnPoint.CurrentValue.Start / 100), 1, Layer.Bounds.Width);
                            y = Math.Clamp((int)(Layer.Bounds.Height * Properties.RippleSpawnPoint.CurrentValue.End / 100), 1, Layer.Bounds.Height);
                            break;
                        case RippleSpawnLocation.BottomLeft:
                            x = 1;
                            y = Layer.Bounds.Height;
                            break;
                        case RippleSpawnLocation.BottomRight:
                            x = Layer.Bounds.Width;
                            y = Layer.Bounds.Height;
                            break;
                        case RippleSpawnLocation.Centre:
                            x = Layer.Bounds.Width / 2;
                            y = Layer.Bounds.Height / 2;
                            break;
                        case RippleSpawnLocation.MiddleLeft:
                            x = 1;
                            y = Layer.Bounds.Height / 2;
                            break;
                        case RippleSpawnLocation.MiddleRight:
                            x = Layer.Bounds.Width;
                            y = Layer.Bounds.Height / 2;
                            break;
                        case RippleSpawnLocation.TopCentre:
                            x = Layer.Bounds.Width / 2;
                            y = 1;
                            break;
                        case RippleSpawnLocation.TopLeft:
                            x = 1;
                            y = 1;
                            break;
                        case RippleSpawnLocation.TopRight:
                            x = Layer.Bounds.Width;
                            y = 1;
                            break;
                        case RippleSpawnLocation.BottomCentre:
                            x = Layer.Bounds.Width / 2;
                            y = Layer.Bounds.Height;
                            break;
                    };

                    SKPoint spawnPoint = new SKPoint(x, y);
                    SpawnEffect(spawnPoint);
                }
                _lastSpawnTime = 0;
            }

            lock (_ripples)
            {
                _ripples.Where(w => w.Finished).ToList().ForEach(r => r.Dispose());
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

            // TODO: Use a ripple pull
            lock (_ripples)
            {
                _ripples.Add(new Ripple(this, relativeLedPosition));
            }
        }
    }
}
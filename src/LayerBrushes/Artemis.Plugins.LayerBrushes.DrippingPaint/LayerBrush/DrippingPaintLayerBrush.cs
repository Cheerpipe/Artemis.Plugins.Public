using System;
using System.Collections.Generic;
using System.Linq;
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.DrippingPaint.LayerBrush.DrippingPaint;
using Artemis.Plugins.LayerBrushes.DrippingPaint.LayerProperties;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.DrippingPaint.LayerBrush
{
    public class DrippingPaintLayerBrush : LayerBrush<DrippingPaintLayerBrushProperties>
    {
        #region Properties & Fields

        private readonly List<DrippingLayer> _layers = new();
        private readonly Profiler _profiler;
        private Random Rand { get; set; }
        private float _totalTime;

        #endregion

        #region Constructors

        public DrippingPaintLayerBrush(Plugin plugin)
        {
            _profiler = plugin.GetProfiler("DrippingPaintLayerBrush");
        }

        #endregion

        #region Plugin lifecycle methods

        public override void EnableLayerBrush()
        {
            Rand = new Random(Layer.EntityId.GetHashCode());
            _layers.Add(CreateDrippingLayer());
        }

        public override void DisableLayerBrush()
        {
            _layers.Clear();
        }

        #endregion

        #region Plugin methods
        public DrippingLayer CreateDrippingLayer()
        {
            DrippingLayer drippingLayer = new();
            if (Layer.Bounds.Width == 0)
            {
                return drippingLayer;
            }

            float distance = (float)Layer.Bounds.Width / Properties.DripCount.CurrentValue;
            drippingLayer.Points.Clear();

            for (int i = 0; i <= Properties.DripCount.CurrentValue; i++)
            {
                DrippingPoint p = new DrippingPoint
                {
                    XPosition = i * distance,
                    YPosition = 0,
                    Acceleration = Properties.DropGravity.CurrentValue.GetRandomValue() / 100f
                };
                drippingLayer.Points.Add(p);
            }

            drippingLayer.Color = Properties.ColorMode.CurrentValue switch
            {
                ColorType.ColorSet => Properties.Colors.CurrentValue.GetColor((float)Rand.NextDouble()),
                _ => SKColor.FromHsv(Rand.Next(0, 360), 100, 100)
            };

            return drippingLayer;
        }

        public override void Update(double deltaTime)
        {
            lock (this)
            {
                _profiler.StartMeasurement("Update");
                // Keep at least 3 background layers to avoid black flickering. It is faster than calculate what layer is on top and if it is covering all the surface because it uses arcs.
                int toRemoveLayerCount = _layers.FindLastIndex(l => l.Points.Count > 0 && !l.Points.Exists(p => p.YPosition < Layer.Bounds.Height)) - 3;
                _layers.RemoveRange(0, Math.Max(toRemoveLayerCount, 0));

                if (deltaTime < 0)
                {
                    return;
                }

                foreach (DrippingPoint point in _layers.SelectMany(l => l.Points))
                {
                    point.Fall((float)deltaTime, Properties.InitialSpeed.CurrentValue);
                }

                // Create a new paint wave
                switch (Properties.DrippingTrigger.CurrentValue)
                {
                    case DrippingTrigger.Threshold:
                        if (!_layers.Last().Points.Exists(p => (p.YPosition) < this.Layer.Bounds.Height * (Properties.Threshold.CurrentValue / 100f)))
                        {
                            _layers.Add(CreateDrippingLayer());
                        }
                        break;
                    case DrippingTrigger.Delay:
                        _totalTime += (float)deltaTime;
                        if (_totalTime > (Properties.Delay.CurrentValue / 1000f))
                        {
                            _totalTime = 0;
                            _layers.Add(CreateDrippingLayer());
                        }

                        break;
                }
                _profiler.StopMeasurement("Update"); 
            }
        }

        public override void Render(SKCanvas canvas, SKRect bounds, SKPaint paint)
        {
            lock (this)
            {
                _profiler.StartMeasurement("Render");
                canvas.Clear();
                foreach (DrippingLayer drippingLayer in _layers)
                {
                    SKPath path = new SKPath();
                    path.MoveTo(0, 0);
                    paint.Color = drippingLayer.Color;
                    int lastX = 0;

                    for (int i = 0; i < drippingLayer.Points.Count; i++)
                    {
                        SKPoint cornerPoint = new SKPoint(drippingLayer.Points[i].XPosition, drippingLayer.Points[i].YPosition);
                        lastX = Math.Max(lastX, (int)cornerPoint.X);
                        SKPoint destinationPoint;

                        if (i == drippingLayer.Points.Count - 1)
                        {
                            destinationPoint = cornerPoint;
                        }
                        else
                            destinationPoint = new SKPoint(drippingLayer.Points[i + 1].XPosition, drippingLayer.Points[i + 1].YPosition);

                        float maxRadius = (float)Layer.Bounds.Width / Properties.DripCount.CurrentValue / 2f;
                        float yDistance = Math.Abs(cornerPoint.Y - path.LastPoint.Y);
                        float yDistanceSubtraction = yDistance - maxRadius;
                        float radius = Math.Clamp(maxRadius - yDistanceSubtraction, maxRadius / 2, maxRadius);
                        path.ArcTo(cornerPoint, destinationPoint, radius);
                    }

                    path.LineTo(lastX, 0);
                    path.Close();
                    canvas.DrawPath(path, paint);
                    path.Dispose();
                }
                _profiler.StopMeasurement("Render");
            }
        }

        #endregion
    }
}
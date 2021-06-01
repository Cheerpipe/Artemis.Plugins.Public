using System;
using System.Collections.Generic;
using Artemis.Core;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.ConnectingDots.ConnectingDots;
using Artemis.Plugins.LayerBrushes.ConnectingDots.LayerProperties;
using Artemis.Plugins.LayerBrushes.ConnectingDots.LayerProperties.Presets;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.ConnectingDots
{
    public class ConnectingDotsBrush : LayerBrush<ConnectingDotsBrushProperties>
    {
        private readonly Plugin _plugin;
        private readonly Profiler _profiler;
        private Field _field;
        private float _gradientAdvance;
        private float _fieldAdvance;
        public override void EnableLayerBrush()
        {
        }

        public override List<ILayerBrushPreset> Presets => new()
        {
            new ElectricField(this),
            new DancingFireLines(this, _plugin),
            new BigBouncingBubbles(this),
            new SmallBouncingBubbles(this),
        };

        public ConnectingDotsBrush(Plugin plugin)
        {
            _plugin = plugin;
            _profiler = plugin.GetProfiler("ConnectingDotsBrush");
        }

        public override void DisableLayerBrush()
        {
            _field = null;
        }

        public override void Update(double deltaTime)
        {
            if (_field == null)
                return;
            _profiler.StartMeasurement("Update");
            _fieldAdvance = (float)(deltaTime * Properties.DotsMovementSpeed.CurrentValue);

            if (Math.Abs(_fieldAdvance) <= (Properties.Radius.CurrentValue / 2f))
                _field.Advance(_fieldAdvance);

            _gradientAdvance = (float)Math.Abs((deltaTime * Properties.ColorChangeSpeed.CurrentValue));
            _profiler.StopMeasurement("Update");
        }

        public override void Render(SKCanvas canvas, SKRect bounds, SKPaint paint)
        {
            _profiler.StartMeasurement("Render");

            _field ??= new Field(bounds.Width, bounds.Height, Properties.Dots.CurrentValue);

            _field.DotCount = Properties.Dots.CurrentValue;
            _field.Width = bounds.Width;
            _field.Height = bounds.Height;

            canvas.Clear(Properties.Background.CurrentValue);

            double minConnectDistance = Properties.ConnectDistance.CurrentValue.Start;
            double maxConnectDistance = Properties.ConnectDistance.CurrentValue.End;

            foreach (var dot1 in _field.Dots)
            {
                foreach (var dot2 in _field.Dots)
                {
                    if (dot1.Y >= dot2.Y)
                        continue; // prevent duplicate lines

                    double dX = Math.Abs(dot1.X - dot2.X);
                    double dY = Math.Abs(dot1.Y - dot2.Y);

                    if (dX < minConnectDistance || dX > maxConnectDistance ||
                        dY < minConnectDistance || dY > maxConnectDistance)
                        continue;

                    if (!(Properties.ConnectionWidth.CurrentValue > 0)) continue;
                    paint.Color = Properties.Connections.CurrentValue;
                    paint.StrokeWidth = Properties.ConnectionWidth.CurrentValue;
                    if (dX > minConnectDistance && dX < maxConnectDistance &
                        dY > minConnectDistance && dY < maxConnectDistance)
                    {
                        canvas.DrawLine(dot1.X, dot1.Y, dot2.X, dot2.Y, paint);
                    }
                }
            }

            float dotRadius = Properties.Radius.CurrentValue;
            foreach (var dot in _field.Dots)
            {
                var pt = new SKPoint(dot.X, dot.Y);
                if (Properties.DotsColorType.CurrentValue == ConnectingDotsBrushProperties.ColorMappingType.Gradient)
                {
                    var cpos = dot.GetColorPercentageAndMove(_gradientAdvance);
                    paint.Color = Properties.Colors.CurrentValue.GetColor(cpos);
                }
                else
                {
                    paint.Color = Properties.Color.CurrentValue;
                }
                canvas.DrawCircle(pt, dotRadius, paint);
            }
            _fieldAdvance = 0;
            _gradientAdvance = 0;

            _profiler.StopMeasurement("Render");
        }
    }
}
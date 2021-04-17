using System;
using System.Drawing;
using System.Threading;
using Artemis.Core.LayerBrushes;
using ConnectingDots;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.ConnectingDots
{
    public class ConnectingDotsBrush : LayerBrush<ConnectingDotsBrushProperties>
    {
        private Field _field;
        private float _gradientAdvance;
        private float _fieldAdvance;
        public override void EnableLayerBrush()
        {
        }

        public override void DisableLayerBrush()
        {
            _field = null;
        }

        public override void Update(double deltaTime)
        {
            if (_field == null)
                return;

            _fieldAdvance = (float)(deltaTime * Properties.DotsMovementSpeed.CurrentValue);

            if (Math.Abs(_fieldAdvance) <= (Properties.Radius.CurrentValue / 2f))
                _field.Advance(_fieldAdvance);

            _gradientAdvance = (float)Math.Abs((deltaTime * Properties.ColorChangeSpeed.CurrentValue));
        }

        public override void Render(SKCanvas canvas, SKRect bounds, SKPaint paint)
        {
            if (_field is null)
                _field = new Field(bounds.Width, bounds.Height, Properties.Dots.CurrentValue);

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
                    double distance = Math.Sqrt(dX * dX + dY * dY);
                    int alpha = (int)(255 - distance / maxConnectDistance * 255) * 2;
                    alpha = Math.Min(alpha, 255);
                    alpha = Math.Max(alpha, 0);
                    if (Properties.ConnectionWidth.CurrentValue > 0)
                    {
                        paint.Color = Properties.Connections.CurrentValue;
                        paint.StrokeWidth = Properties.ConnectionWidth.CurrentValue;
                        if (dX > minConnectDistance && dX < maxConnectDistance &
                            dY > minConnectDistance && dY < maxConnectDistance)
                        {
                            canvas.DrawLine(dot1.X, dot1.Y, dot2.X, dot2.Y, paint);
                        }
                    }
                }
            }

            float dotRadius = Properties.Radius.CurrentValue;
            foreach (var dot in _field.Dots)
            {
                var rect = new RectangleF(
                        x: dot.X - dotRadius,
                        y: dot.Y - dotRadius,
                        width: dotRadius * 2,
                        height: dotRadius * 2
                    );

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

        }
    }
}
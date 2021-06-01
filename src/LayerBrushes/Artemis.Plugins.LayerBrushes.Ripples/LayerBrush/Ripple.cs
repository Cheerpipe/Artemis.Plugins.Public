using System;
using Artemis.Core;
using Artemis.Plugins.LayerBrushes.Ripples.LayerProperties;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerBrush
{
    public class Ripple : IDisposable
    {
        private readonly RipplesLayerBrush _brush;
        private SKPaint _paint;
        private float _progress;
        private SKColor _trailColor;
        private SKPaint _trailPaint;
        private Random _rand;

        public Ripple(RipplesLayerBrush brush, SKPoint position)
        {
            _brush = brush;
            Position = position;
            Expand = true;
            UpdatePaint();
        }
        public float Size { get; set; }
        public bool Expand { get; set; }

        public void UpdateOne(double deltaTime)
        {
            if (Expand)
                Size += (float)(deltaTime * _brush.Properties.RippleGrowthSpeed.CurrentValue);
            else
                Size = -1;

            if (Size > _brush.Properties.RippleSize) Expand = false;

            UpdatePaint();
        }

        private void UpdatePaint()
        {
            if (_brush.Properties.ColorMode.CurrentValue == ColorType.Random && _paint == null)
            {
                _paint = new SKPaint { Color = SKColor.FromHsv(_brush.Rand.Next(0, 360), 100, 100) };
            }
            else if (_brush.Properties.ColorMode.CurrentValue == ColorType.ColorSet && _paint == null)
            {

                _rand ??= new Random(GetHashCode());
                _paint = new SKPaint { Color = _brush.Properties.Colors.CurrentValue.GetColor((float)_rand.NextDouble()) };
            }
            else if (_brush.Properties.ColorMode.CurrentValue == ColorType.Solid)
            {
                _paint = new SKPaint { Color = _brush.Properties.Color.CurrentValue };
            }
            else if (_brush.Properties.ColorMode.CurrentValue == ColorType.Gradient)
            {
                _paint = new SKPaint
                {
                    Shader = SKShader.CreateRadialGradient(
                        Position,
                        _brush.Properties.RippleWidth,
                        _brush.Properties.Colors.BaseValue.GetColorsArray(),
                        _brush.Properties.Colors.BaseValue.GetPositionsArray(),
                        SKShaderTileMode.Repeat
                    )
                };
            }
            else if (_brush.Properties.ColorMode.CurrentValue == ColorType.ColorPathChange)
            {
                _paint = new SKPaint { Color = _brush.Properties.Colors.CurrentValue.GetColor(_progress) };
            }

            byte alpha = 255;
            // Add fade away effect
            if (_brush.Properties.RippleFadeAway != RippleFadeOutMode.None)
                alpha = (byte)(255d * Easings.Interpolate(1f - _progress, (Easings.Functions)_brush.Properties.RippleFadeAway.CurrentValue));

            // If we have to paint a trail
            if (_brush.Properties.RippleTrail)
            {
                // Moved trail color calculation here to avoid extra overhead when trail is not enabled
                _trailColor = _brush.Properties.ColorMode.CurrentValue switch
                {
                    // If gradient is used, calculate the inner color to a given position.
                    ColorType.Gradient => _brush.Properties.Colors.CurrentValue.GetColor((Size - _brush.Properties.RippleWidth / 2f) % _brush.Properties.RippleWidth / _brush.Properties.RippleWidth),
                    // If not gradient, we can just copy the color of the ripple Paint.
                    _ => _paint.Color
                };

                // Dispose before to create a new one. Thanks for the lesson.
                _trailPaint = new SKPaint
                {
                    Shader = SKShader.CreateRadialGradient(
                        Position,
                        Size,
                        // Trail is simply a gradient from full inner ripple color to the same color but with alpha 0. Just an illution :D
                        new[] { _trailColor.WithAlpha(0), _trailColor.WithAlpha(alpha) },
                        new[] { 0f, 1f },
                        SKShaderTileMode.Clamp
                    )
                };
                _trailPaint.Style = SKPaintStyle.Fill;
            }

            // Set ripple size and final color alpha
            _paint.Color = _paint.Color.WithAlpha(alpha);
            _paint.Style = SKPaintStyle.Stroke;
            _paint.StrokeWidth = _brush.Properties.RippleWidth.CurrentValue;
        }

        public bool Finished => _progress > 1;
        public ArtemisLed Led { get; }
        public SKPoint Position { get; set; }

        public void Update(double deltaTime)
        {
            UpdateOne(deltaTime);
            _progress = Size / _brush.Properties.RippleSize;
        }

        public void Render(SKCanvas canvas)
        {
            // Animation finished. Nothing to see here.
            if (Size < 0)
                return;

            // SkiaSharp shapes doesn't support inner stroke so it will cause a weird empty circle if stroke
            // width is greather than the double of the size of a shape. This operation will produce perfect ripples
            _paint.StrokeWidth = Math.Min(_paint.StrokeWidth, Size * 2);

            if (Size > 0 && _paint != null)
                canvas.DrawCircle(Position, Size, _paint);

            // Draw the trail
            if (_brush.Properties.RippleTrail)
                // Start from end of ripple circle and ensure radios is never 0
                canvas.DrawCircle(Position, Math.Max(0, Size - _brush.Properties.RippleWidth.CurrentValue / 2f), _trailPaint);
        }

        public void Dispose()
        {
            _paint.Dispose();
            _trailPaint.Dispose();
        }
    }
}
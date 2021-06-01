using System;
using Artemis.Core;
using Artemis.Plugins.LayerBrushes.Ripples.LayerProperties;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Ripples.LayerBrush
{
    public sealed class Ripple : IDisposable
    {
        private readonly RipplesLayerBrush _brush;
        private readonly SKPaint _paint;
        private readonly SKPaint _trailPaint;
        private float _progress;
        private bool _colorFixed;
        private static Random _random;

        public Ripple(RipplesLayerBrush brush, SKPoint position)
        {
            _brush = brush;
            Position = position;
            Expand = true;
            _paint = new SKPaint();
            _trailPaint = new SKPaint();
            _random = new Random(GetHashCode());
        }

        private float Size { get; set; }
        private bool Expand { get; set; }

        private void UpdatePaint()
        {
            if (_brush.Properties.ColorMode.CurrentValue == ColorType.Random && !_colorFixed)
            {
                _colorFixed = true;
                _paint.Color = SKColor.FromHsv(_brush.Rand.Next(0, 360), 100, 100);
            }
            else if (_brush.Properties.ColorMode.CurrentValue == ColorType.ColorSet && !_colorFixed)
            {
                _colorFixed = true;
                _paint.Color = _brush.Properties.Colors.CurrentValue.GetColor((float)_random.NextDouble());
            }
            else if (_brush.Properties.ColorMode.CurrentValue == ColorType.Solid && !_colorFixed)
            {
                _paint.Color = _brush.Properties.Color.CurrentValue;
            }
            else if (_brush.Properties.ColorMode.CurrentValue == ColorType.Gradient)
            {
                _paint?.Shader?.Dispose();
                _paint.Shader = SKShader.CreateRadialGradient
                    (
                        Position,
                        _brush.Properties.RippleWidth,
                        _brush.Properties.Colors.BaseValue.GetColorsArray(),
                        _brush.Properties.Colors.BaseValue.GetPositionsArray(),
                        SKShaderTileMode.Repeat
                    );
            }
            else if (_brush.Properties.ColorMode.CurrentValue == ColorType.ColorPathChange)
            {
                _paint.Color = _brush.Properties.Colors.CurrentValue.GetColor(_progress);
            }

            byte alpha = 255;
            // Add fade away effect
            if (_brush.Properties.RippleFadeAway != RippleFadeOutMode.None)
                alpha = (byte)(255d * Easings.Interpolate(1f - _progress, (Easings.Functions)_brush.Properties.RippleFadeAway.CurrentValue));


            SKColor trailColor;
            // If we have to paint a trail
            if (_brush.Properties.RippleTrail)
            {
                // Moved trail color calculation here to avoid extra overhead when trail is not enabled
                trailColor = _brush.Properties.ColorMode.CurrentValue switch
                {
                    // If gradient is used, calculate the inner color to a given position.
                    ColorType.Gradient => _brush.Properties.Colors.CurrentValue.GetColor((Size - _brush.Properties.RippleWidth / 2f) % _brush.Properties.RippleWidth / _brush.Properties.RippleWidth),
                    // If not gradient, we can just copy the color of the ripple Paint.
                    _ => _paint.Color
                };

                // Dispose before to create a new one. Thanks for the lesson.
                _trailPaint?.Shader?.Dispose();
                _trailPaint.Shader = SKShader.CreateRadialGradient
                    (
                        Position,
                        Size,
                        // Trail is simply a gradient from full inner ripple color to the same color but with alpha 0. Just an illusion :D
                        new[] { trailColor.WithAlpha(0), trailColor.WithAlpha(alpha) },
                        new[] { 0f, 1f },
                        SKShaderTileMode.Clamp
                    );
                _trailPaint.Style = SKPaintStyle.Fill;
            }

            // Set ripple size and final color alpha
            _paint.Color = _paint.Color.WithAlpha(alpha);
            _paint.Style = SKPaintStyle.Stroke;

            // SkiaSharp shapes doesn't support inner stroke so it will cause a weird empty circle if stroke
            // width is greater than the double of the size of a shape. This operation will produce perfect ripples
            _paint.StrokeWidth = Math.Min(_brush.Properties.RippleWidth.CurrentValue, Size * 2);
        }

        public bool Finished => _progress >= 1;
        private SKPoint Position { get; }

        public void Update(double deltaTime)
        {
            if (Expand)
                Size += (float)(deltaTime * _brush.Properties.RippleGrowthSpeed.CurrentValue);
            else
                Size = -1;

            if (Size > _brush.Properties.RippleSize) Expand = false;

            _progress = Math.Min(1, Size / _brush.Properties.RippleSize);
        }

        public void Render(SKCanvas canvas)
        {
            // Animation finished. Nothing to see here.
            if (Finished)
                return;

            UpdatePaint();

            canvas.DrawCircle(Position, Size, _paint);

            // Draw the trail
            if (_brush.Properties.RippleTrail)
                // Start from end of ripple circle and ensure radios is never 0
                canvas.DrawCircle(Position, Math.Max(0, Size - _brush.Properties.RippleWidth.CurrentValue / 2f), _trailPaint);
        }

        public void Dispose()
        {
            _paint?.Shader?.Dispose();
            _paint?.Dispose();
            _trailPaint?.Shader?.Dispose();
            _trailPaint?.Dispose();
        }
    }
}
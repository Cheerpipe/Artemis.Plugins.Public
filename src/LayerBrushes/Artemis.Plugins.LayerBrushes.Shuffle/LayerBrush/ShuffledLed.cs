using System;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush
{
    public sealed class ShuffledLed
    {
        private readonly ShuffleLayerBrush _brush; // Color to be returned
        private float _advance; // How much we move inside the gradient
        private float _lastChangeTime; // used to trigger a new ToColor change
        private Random Rand { get; }
        private SKColor FromColor { get; set; }
        private SKColor ToColor { get; set; }
        private float Speed { get; set; }

        private SKColor InterpolateColor(SKColor fromColor, SKColor toColor, float position)
        {
            byte a = (byte)((toColor.Alpha - fromColor.Alpha) * position + fromColor.Alpha);
            byte r = (byte)((toColor.Red - fromColor.Red) * position + fromColor.Red);
            byte g = (byte)((toColor.Green - fromColor.Green) * position + fromColor.Green);
            byte b = (byte)((toColor.Blue - fromColor.Blue) * position + fromColor.Blue);
            return new SKColor(r, g, b, a);
        }

        public ShuffledLed(ShuffleLayerBrush brush)
        {
            Rand = new Random(GetHashCode());
            _brush = brush;
            SetNextValues();
        }

        public SKColor GetCurrentColor()
        {
            return _brush.Properties.SmoothColorChange ? InterpolateColor(FromColor, ToColor, Math.Clamp(_advance, 0, 1)) : ToColor;
        }

        private void SetNextValues()
        {
            FromColor = ToColor;
            ToColor = _brush.Properties.Colors.CurrentValue.GetColor((float)Rand.NextDouble());
            Speed = _brush.Properties.ChangeSpeed.CurrentValue.GetRandomValue();
        }

        public void Advance(float amount)
        {
            _lastChangeTime += amount;

            // Time to change the From and To colors
            if (_lastChangeTime > 100f / Speed) // 100 is one sec
            {
                SetNextValues();
                _lastChangeTime = 0;
                _advance = 0;
            }
            _advance += amount / (1 / (Speed / 100f));
        }
    }
}
using System;
using Artemis.Core;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.Shuffle.LayerBrush
{
    public class ShufledLed
    {
        private readonly ShuffleLayerBrush _brush; // Color to be returned
        private float _advance; // How much we move inside the gradient
        private float _lastChangeTime; // used to trigger a new ToColor change
        private Random Rand { get; set; }
        public SKColor FromColor { get; set; }
        public SKColor ToColor { get; set; }
        public float Speed { get; set; }

        private SKColor InterpolateColor(SKColor FromColor, SKColor ToColor, float position)
        {
            byte a = (byte)((ToColor.Alpha - FromColor.Alpha) * position + FromColor.Alpha);
            byte r = (byte)((ToColor.Red - FromColor.Red) * position + FromColor.Red);
            byte g = (byte)((ToColor.Green - FromColor.Green) * position + FromColor.Green);
            byte b = (byte)((ToColor.Blue - FromColor.Blue) * position + FromColor.Blue);
            return new SKColor(r, g, b, a);
        }

        public ShufledLed(ShuffleLayerBrush brush)
        {
            Rand = new Random(GetHashCode());
            _brush = brush;
            SetNextValues();
        }

        public SKColor GetCurrentColor()
        {
            if (_brush.Properties.SmoothColorChange)
                return InterpolateColor(FromColor, ToColor, Math.Clamp(_advance, 0, 1));
            else
                return ToColor;
        }

        private void SetNextValues()
        {
            FromColor = ToColor;
            ToColor = _brush.Properties.Colors.CurrentValue.GetColor((float)Rand.NextDouble());
            Speed = _brush.Properties.ChangeSpeed.CurrentValue.GetRandomValue();
        }

        public void Advance(float amount)
        {
            _lastChangeTime += (float)amount;

            // Time to change the From and To colors
            if (_lastChangeTime > 100f / Speed) // 100 is one sec
            {
                SetNextValues();
                _lastChangeTime = 0;
                _advance = 0;
            }
            _advance += (float)amount / (1 / (Speed / 100f));
        }
    }
}
using System;
using Artemis.Core.LayerBrushes;
using Artemis.Plugins.LayerBrushes.SolidPercentage.PropertyGroups;
using SkiaSharp;

namespace Artemis.Plugins.LayerBrushes.SolidPercentage.Brushes
{
    public class SolidPercentageBrush : LayerBrush<SolidPercentageBrushProperties>
    {
        private SKColor _currentColor;
        private float _expandedMinValue = float.MaxValue;
        private float _expandedMaxValue = float.MinValue;

        public override void Render(SKCanvas canvas, SKRect bounds, SKPaint paint)
        {
            paint.Shader = SKShader.CreateColor(_currentColor);
            canvas.DrawRect(bounds, paint);
            paint.Shader?.Dispose();
            paint.Shader = null;
        }

        public override void EnableLayerBrush()
        {
        }

        public override void DisableLayerBrush()
        {
        }

        public override void Update(double deltaTime)
        {
            float minValue = Properties.MinAndMax.CurrentValue.Start;
            float maxValue = Properties.MinAndMax.CurrentValue.End;
            float currentValue = Properties.CurrentValue.CurrentValue;

            if (Properties.AutoExpandMinValue.CurrentValue)
            {
                minValue = Math.Min(currentValue, minValue);
                minValue = Math.Min(_expandedMinValue, minValue);
                _expandedMinValue=minValue;
            }

            if (Properties.AutoExpandMaxValue.CurrentValue)
            {
                maxValue = Math.Max(currentValue, maxValue);
                maxValue = Math.Max(_expandedMaxValue, maxValue);
                _expandedMaxValue = maxValue;
            }

            float range = maxValue - minValue;
            float percent = (currentValue - minValue) / range;
            _currentColor = Properties.Colors.CurrentValue.GetColor(percent);
        }
    }
}
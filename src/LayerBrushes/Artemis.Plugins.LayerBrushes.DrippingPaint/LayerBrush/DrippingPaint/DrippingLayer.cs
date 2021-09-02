using SkiaSharp;
using System.Collections.Generic;

namespace Artemis.Plugins.LayerBrushes.DrippingPaint.LayerBrush.DrippingPaint
{
    public class DrippingLayer
    {
        public List<DrippingPoint> Points { get; set; } = new List<DrippingPoint>();
        public SKColor Color { get; set; }
    }
}

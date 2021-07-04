using System;
using Artemis.Core.LayerEffects;
using Artemis.Plugins.LayerEffect.FlickeringLights.PropertyGroups;
using SkiaSharp;

namespace Artemis.Plugins.LayerEffect.FlickeringLights
{
    public class FlickeringLightsPluginLayerEffect : LayerEffect<MainPropertyGroup>
    {
        private float _progress;
        private float _alpha;

        public override void EnableLayerEffect() { }

        public override void DisableLayerEffect() { }

        public override void Update(double deltaTime)
        {
            var loopTime = Math.Max(Properties.LoopTime.CurrentValue, 0.1f);
            _progress += (float)deltaTime;
            _alpha = GetNextAlpha(_progress / loopTime);
            if (_progress > loopTime)
                _progress = 0;
        }

        private float GetNextAlpha(float position)
        {
            position = Math.Clamp(position, 0, 1);
            var charIndex = (int)Math.Round(position * (Properties.FlickeringPattern.CurrentValue.ToString().Length - 1), 0);
            return ((Properties.FlickeringPattern.CurrentValue.ToString()[charIndex] - 'a') / 25f) * 2;
        }


        public override void PreProcess(SKCanvas canvas, SKRect renderBounds, SKPaint paint)
        {
            paint.ColorF = paint.ColorF.WithAlpha(_alpha);
        }

        public override void PostProcess(SKCanvas canvas, SKRect renderBounds, SKPaint paint) { }

        private static float Mod(float x, float m)
        {
            return (x % m + m) % m;
        }
    }
}
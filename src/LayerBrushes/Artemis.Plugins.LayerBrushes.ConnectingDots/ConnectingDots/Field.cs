using System;
using System.Collections.Generic;

namespace Artemis.Plugins.LayerBrushes.ConnectingDots.ConnectingDots
{
    public class Field
    {
        private readonly List<Dot> _dots = new();
        public IEnumerable<Dot> Dots => _dots;
        private float _width;
        public float Width { set => SetNewWidth(value); }

        private float _height;
        public float Height { set => SetNewHeight(value); }

        private int _dotCount;
        public int DotCount { set => SetDotCount(value); }

        private static readonly Random Rand = new();

        public Field(float width, float height, int dotCount)
        {
            SetNewWidth(width);
            SetNewHeight(height);
            SetDotCount(dotCount);
        }

        void SetNewWidth(float width)
        {
            _width = width;
            foreach (var t in _dots)
            {
                if (t.X >= _width)
                    t.X = _width;
                else if (t.X < 0)
                    t.X = 0;
            }
        }

        void SetNewHeight(float height)
        {
            _height = height;
            foreach (var t in _dots)
            {
                if (t.Y >= _height)
                    t.Y = _height;
                else if (t.Y < 0)
                    t.Y = 0;
            }
        }

        void SetDotCount(int dotCount)
        {
            if (_dotCount == dotCount)
                return;

            _dotCount = dotCount;

            if (_dots.Count > dotCount)
            {
                _dots.RemoveRange(dotCount, _dots.Count - dotCount);
            }
            else if (_dots.Count < dotCount)
            {
                while (_dots.Count < dotCount)
                {
                    _dots.Add(new Dot()
                    {
                        X = (float)(Rand.NextDouble() * (0.7f - 0.3f) + 0.3f) * _width,
                        Xvel = .1f + (float)Rand.NextDouble() * (Rand.NextDouble() > .5f ? -1f : 1f),
                        Y = (float)(Rand.NextDouble() * (0.7f - 0.3f) + 0.3f) * _height,
                        Yvel = .1f + (float)Rand.NextDouble() * (Rand.NextDouble() > .5f ? -1f : 1f),
                        ColorPercentage = (float)Rand.NextDouble() * 100
                    });
                }
            }
        }

        public void Advance(float multiplier = .1f)
        {
            foreach (var t in _dots)
            {
                var tmpX = t.X + t.Xvel * multiplier;
                var tmpY = t.Y + t.Yvel * multiplier;

                t.X = tmpX;
                t.Y = tmpY;

                bool outOfBoundsX = (t.X < 0) || (t.X >= _width);
                bool outOfBoundsY = (t.Y < 0) || (t.Y >= _height);

                if (outOfBoundsX) t.Xvel *= -1;
                if (outOfBoundsY) t.Yvel *= -1;
            }
        }
    }
}

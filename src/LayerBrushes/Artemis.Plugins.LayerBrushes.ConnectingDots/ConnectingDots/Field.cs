using System;
using System.Collections.Generic;
using System.Threading;

namespace ConnectingDots
{
    public class Field
    {
        private List<Dot> _dots = new List<Dot>();
        public List<Dot> Dots { get => _dots; }
        private float _width { get; set; }
        public float Width { get => _width; set => SetNewWidth(value); }

        private float _height { get; set; }
        public float Height { get => _height; set => SetNewHeight(value); }

        private int _dotCount;
        public int DotCount { get => _dotCount; set => SetDotCount(value); }

        private static readonly Random rand = new Random();

        public Field(float width, float height, int dotCount)
        {
            SetNewWidth(width);
            SetNewHeight(height);
            SetDotCount(dotCount);
        }

        void SetNewWidth(float width)
        {
            _width = width;
            for (int i = 0; i < _dots.Count; i++)
            {
                if (_dots[i].X >= _width)
                    _dots[i].X = _width;
                else if (_dots[i].X < 0)
                    _dots[i].X = 0;
            }
        }

        void SetNewHeight(float height)
        {
            _height = height;
            for (int i = 0; i < _dots.Count; i++)
            {
                if (_dots[i].Y >= _height)
                    _dots[i].Y = _height;
                else if (_dots[i].Y < 0)
                    _dots[i].Y = 0;
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
                        X = (float)(rand.NextDouble() * (0.7f - 0.3f) + 0.3f) * _width,
                        Xvel = .1f + (float)rand.NextDouble() * (rand.NextDouble() > .5f ? -1f : 1f),
                        Y = (float)(rand.NextDouble() * (0.7f - 0.3f) + 0.3f) * _height,
                        Yvel = .1f + (float)rand.NextDouble() * (rand.NextDouble() > .5f ? -1f : 1f),
                        ColorPercentage = (float)rand.NextDouble() * 100
                    }); ;
                }
            }
        }

        public void Advance(float multiplier = .1f)
        {
            for (int i = 0; i < _dots.Count; i++)
            {
                var tmp_x = _dots[i].X + _dots[i].Xvel * multiplier;
                var tmp_y = _dots[i].Y + _dots[i].Yvel * multiplier;

                _dots[i].X = tmp_x;
                _dots[i].Y = tmp_y;

                bool outOfBoundsX = (_dots[i].X < 0) || (_dots[i].X >= _width);
                bool outOfBoundsY = (_dots[i].Y < 0) || (_dots[i].Y >= _height);

                if (outOfBoundsX) _dots[i].Xvel *= -1;
                if (outOfBoundsY) _dots[i].Yvel *= -1;
            }
        }
    }
}

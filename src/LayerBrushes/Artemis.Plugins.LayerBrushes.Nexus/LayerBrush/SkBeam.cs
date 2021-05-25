using Artemis.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;

namespace Artemis.Plugins.LayerBrushes.Nexus.LayerBrush
{
    public class SkBeam
    {
        static Random random = new Random();

        public Direction Direction { get; private set; }
        public float Width { get; private set; }
        public float Position { get; private set; }
        public int Location { get; private set; }
        public float Speed { get; private set; }
        public ColorGradient Colors { get; set; }
        public bool HasCustomGradient { get; private set; }

        public SkBeam(Direction direction, int location, float width, ColorGradient colors, float speed)
        {
            Direction = direction;
            Width = width;
            Position = 0;
            Location = location;
            Speed = speed;
            Colors = colors;
        }

        public void Move()
        {
            Position++;
        }

        public void Move(float steps)
        {
            Position += steps;
        }


        public static Direction GetRandomDirection(bool toLeft, bool toUp, bool toRight, bool toBottom)
        {
            List<Direction> values = new List<Direction>();

            if (toLeft) values.Add(Direction.ToLeft);
            if (toUp) values.Add(Direction.ToUp);
            if (toRight) values.Add(Direction.ToRight);
            if (toBottom) values.Add(Direction.ToDown);

            return values[random.Next(values.Count)];
        }
    }

    public enum Direction
    {
        ToLeft = 0,
        ToUp = 1,
        ToRight = 2,
        ToDown = 3
    }
}

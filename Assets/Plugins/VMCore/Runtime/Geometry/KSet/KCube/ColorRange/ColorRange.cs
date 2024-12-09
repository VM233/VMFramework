using System;
using UnityEngine;

namespace VMFramework.Core
{
    public readonly partial struct ColorRange : IKCubeFloat<Color>, IEquatable<ColorRange>, IFormattable
    {
        public static Color ZeroColor { get; } = new(0, 0, 0, 0);

        public static ColorRange Zero { get; } =
            new(ZeroColor, ZeroColor);

        public static ColorRange One { get; } =
            new(Color.white, Color.white);

        public static ColorRange Unit { get; } =
            new(ZeroColor, Color.white);

        public Color Size => max - min;

        public Color Pivot => (max + min) / 2;

        public Color Extents => (max - min) / 2;

        public readonly Color min, max;

        public RangeFloat RRange => new(min.r, max.r);

        public RangeFloat GRange => new(min.g, max.g);

        public RangeFloat BRange => new(min.b, max.b);

        public RangeFloat ARange => new(min.a, max.a);

        #region Constructor

        public ColorRange(RangeFloat xRange, RangeFloat yRange, RangeFloat zRange)
        {
            min = new Color(xRange.min, yRange.min, zRange.min);
            max = new Color(xRange.max, yRange.max, zRange.max);
        }

        public ColorRange(float xMin, float yMin, float zMin, float xMax, float yMax,
            float zMax)
        {
            min = new Color(xMin, yMin, zMin);
            max = new Color(xMax, yMax, zMax);
        }

        public ColorRange(Color min, Color max)
        {
            this.min = min;
            this.max = max;
        }

        public ColorRange(float width, float length, float height)
        {
            min = ZeroColor;
            max = new Color(width, length, height);
        }

        public ColorRange(Color size)
        {
            min = ZeroColor;
            max = size;
        }

        public ColorRange(ColorRange source)
        {
            min = source.min;
            max = source.max;
        }

        public ColorRange(IMinMaxOwner<Color> config)
        {
            if (config is null)
            {
                min = ZeroColor;
                max = ZeroColor;
                return;
            }
            min = config.Min;
            max = config.Max;
        }

        #endregion

        #region Equatable

        public bool Equals(ColorRange other)
        {
            return min.Equals(other.min) && max.Equals(other.max);
        }

        public override bool Equals(object obj)
        {
            return obj is ColorRange other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(min, max);
        }

        #endregion

        #region To String

        public override string ToString() => $"[{min}, {max}]";

        public string ToString(string format, IFormatProvider formatProvider) =>
            $"[{min.ToString(format, formatProvider)},{max.ToString(format, formatProvider)}]";

        #endregion
    }
}

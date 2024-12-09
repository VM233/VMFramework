using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Core
{
    public readonly partial struct RangeInteger : IKCubeInteger<int>, IEquatable<RangeInteger>, IFormattable
    {
        public static RangeInteger Zero { get; } = new(0, 0);

        public static RangeInteger One { get; } = new(1, 1);

        public static RangeInteger Unit { get; } = new(0, 1);

        public int Size => max - min + 1;

        public int Count => Size;

        public int Pivot => (max + min) / 2;

        public readonly int min, max;

        #region Constructor

        public RangeInteger(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public RangeInteger(int length)
        {
            min = 0;
            max = length - 1;
        }

        public RangeInteger(RangeInteger source)
        {
            min = source.min;
            max = source.max;
        }

        public RangeInteger(IMinMaxOwner<int> config)
        {
            if (config is null)
            {
                min = 0;
                max = 0;
                return;
            }

            min = config.Min;
            max = config.Max;
        }

        public RangeInteger(Vector2Int range)
        {
            min = range.x;
            max = range.y;
        }

        #endregion

        #region Equatable

        public bool Equals(RangeInteger other)
        {
            return min.Equals(other.min) && max.Equals(other.max);
        }

        public override bool Equals(object obj)
        {
            return obj is RangeInteger other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(min, max);
        }

        #endregion

        #region To String

        public override string ToString() => $"[{min}, {max}]";

        public string ToString(string format, IFormatProvider formatProvider) =>
            $"[{min.ToString(format, formatProvider)}, {max.ToString(format, formatProvider)}]";

        #endregion
    }
}

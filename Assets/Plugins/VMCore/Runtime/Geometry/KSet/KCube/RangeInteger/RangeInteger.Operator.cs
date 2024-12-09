using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct RangeInteger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out int min, out int max)
        {
            min = this.min;
            max = this.max;
        }
        
        public static RangeInteger operator +(RangeInteger a, int b) =>
            new(a.min + b, a.max + b);

        public static RangeInteger operator -(RangeInteger a, int b) =>
            new(a.min - b, a.max - b);

        public static RangeInteger operator *(RangeInteger a, int b)
        {
            if (b >= 0)
            {
                return new(a.min * b, a.max * b);
            }

            return new(a.max * b, a.min * b);
        }

        public static RangeInteger operator /(RangeInteger a, int b)
        {
            if (b >= 0)
            {
                return new(a.min / b, a.max / b);
            }

            return new(a.max / b, a.min / b);
        }

        public static RangeInteger operator -(RangeInteger a) => new(-a.max, -a.min);

        public static RectangleInteger operator *(RangeInteger a, RangeInteger b) =>
            new(a, b);

        public static bool operator ==(RangeInteger a, RangeInteger b) =>
            a.Equals(b);

        public static bool operator !=(RangeInteger a, RangeInteger b) =>
            !a.Equals(b);

        public static implicit operator RangeInteger(Vector2Int range) => new(range);
        
        public static implicit operator (int min, int max)(RangeInteger range) => (range.min, range.max);
        
        public static implicit operator RangeInteger((int min, int max) range) => new(range.min, range.max);
    }
}
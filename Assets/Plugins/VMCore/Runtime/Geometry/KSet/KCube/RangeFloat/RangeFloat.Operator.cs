using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct RangeFloat
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out float min, out float max)
        {
            min = this.min;
            max = this.max;
        }
        
        public static RangeFloat operator +(RangeFloat a, float b) =>
            new(a.min + b, a.max + b);

        public static RangeFloat operator -(RangeFloat a, float b) =>
            new(a.min - b, a.max - b);

        public static RangeFloat operator *(RangeFloat a, float b)
        {
            if (b >= 0)
            {
                return new(a.min * b, a.max * b);
            }

            return new(a.max * b, a.min * b);
        }


        public static RangeFloat operator /(RangeFloat a, float b)
        {
            if (b >= 0)
            {
                return new(a.min / b, a.max / b);
            }

            return new(a.max / b, a.min / b);
        }

        public static RangeFloat operator -(RangeFloat a) =>
            new(-a.max, -a.min);

        public static RectangleFloat operator *(RangeFloat a, RangeFloat b) =>
            new(a, b);

        public static bool operator ==(RangeFloat a, RangeFloat b) =>
            a.Equals(b);

        public static bool operator !=(RangeFloat a, RangeFloat b) =>
            !a.Equals(b);

        public static implicit operator RangeFloat(Vector2 range) => new(range);
        
        public static implicit operator (float min, float max)(RangeFloat range) => (range.min, range.max);
        
        public static implicit operator RangeFloat((float min, float max) range) => new(range.min, range.max);
    }
}
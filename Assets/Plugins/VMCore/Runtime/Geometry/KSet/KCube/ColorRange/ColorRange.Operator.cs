using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct ColorRange
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out Color min, out Color max)
        {
            min = this.min;
            max = this.max;
        }
        
        public static ColorRange operator +(ColorRange a, Color b) =>
            new(a.min + b, a.max + b);

        public static ColorRange operator -(ColorRange a, Color b) =>
            new(a.min - b, a.max - b);

        public static ColorRange operator *(ColorRange a, Color b) =>
            new(a.min.Multiply(b), a.max.Multiply(b));

        public static ColorRange operator *(ColorRange a, float b) =>
            new(a.min * b, a.max * b);

        public static ColorRange operator /(ColorRange a, Color b) =>
            new(a.min.Divide(b), a.max.Divide(b));

        public static ColorRange operator /(ColorRange a, float b) =>
            new(a.min / b, a.max / b);

        public static bool operator ==(ColorRange a, ColorRange b) =>
            a.Equals(b);

        public static bool operator !=(ColorRange a, ColorRange b) =>
            !a.Equals(b);
        
        public static implicit operator (Color min, Color max)(ColorRange range) => (range.min, range.max);
        
        public static implicit operator ColorRange((Color min, Color max) tuple) => new(tuple.min, tuple.max);
    }
}
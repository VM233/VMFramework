using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class LeftRightDirectionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LeftRightDirection SingleFlagReversed(this LeftRightDirection direction)
        {
            return direction switch
            {
                LeftRightDirection.Left => LeftRightDirection.Right,
                LeftRightDirection.Right => LeftRightDirection.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LeftRightDirection Reversed(this LeftRightDirection direction)
        {
            var result = LeftRightDirection.None;

            if (direction.HasFlag(LeftRightDirection.Left))
            {
                result |= LeftRightDirection.Right;
            }

            if (direction.HasFlag(LeftRightDirection.Right))
            {
                result |= LeftRightDirection.Left;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LeftRightDirection ToLeftRightDirection(this float value)
        {
            return value switch
            {
                < 0 => LeftRightDirection.Left,
                > 0 => LeftRightDirection.Right,
                _ => LeftRightDirection.None
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LeftRightDirection ToLeftRightDirection(this int value)
        {
            return value switch
            {
                < 0 => LeftRightDirection.Left,
                > 0 => LeftRightDirection.Right,
                _ => LeftRightDirection.None
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this LeftRightDirection direction)
        {
            return (int)direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourTypesDirection ToFourTypesDirection(this LeftRightDirection direction)
        {
            return (FourTypesDirection)direction;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightTypesDirection ToEightTypesDirection(this LeftRightDirection direction)
        {
            return (EightTypesDirection)direction;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SixTypesDirection ToSixTypesDirection(this LeftRightDirection direction)
        {
            return (SixTypesDirection)direction;
        }
    }
}

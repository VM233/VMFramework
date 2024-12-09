using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class FourTypesDirectionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourTypesDirection SingleFlagReversed(this FourTypesDirection direction)
        {
            return direction switch
            {
                FourTypesDirection.Left => FourTypesDirection.Right,
                FourTypesDirection.Right => FourTypesDirection.Left,
                FourTypesDirection.Up => FourTypesDirection.Down,
                FourTypesDirection.Down => FourTypesDirection.Up,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourTypesDirection Reversed(this FourTypesDirection direction)
        {
            var result = FourTypesDirection.None;

            if (direction.HasFlag(FourTypesDirection.Up))
            {
                result |= FourTypesDirection.Down;
            }

            if (direction.HasFlag(FourTypesDirection.Down))
            {
                result |= FourTypesDirection.Up;
            }

            if (direction.HasFlag(FourTypesDirection.Left))
            {
                result |= FourTypesDirection.Right;
            }

            if (direction.HasFlag(FourTypesDirection.Right))
            {
                result |= FourTypesDirection.Left;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourTypesDirection ToFourTypesDirection2D(this Vector2 vector)
        {
            if (vector.x.Abs() > vector.y.Abs())
            {
                return vector.x > 0 ? FourTypesDirection.Right : FourTypesDirection.Left;
            }

            if (vector.y.Abs() > vector.x.Abs())
            {
                return vector.y > 0 ? FourTypesDirection.Up : FourTypesDirection.Down;
            }

            return FourTypesDirection.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int ToCardinalVector(this FourTypesDirection direction)
        {
            return direction switch
            {
                FourTypesDirection.Up => Vector2Int.up,
                FourTypesDirection.Down => Vector2Int.down,
                FourTypesDirection.Left => Vector2Int.left,
                FourTypesDirection.Right => Vector2Int.right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourTypesDirection GetOtherDirections(this FourTypesDirection direction)
        {
            return FourTypesDirection.All & ~direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHorizontal(this FourTypesDirection direction)
        {
            return (direction & (FourTypesDirection.Left | FourTypesDirection.Right)) != FourTypesDirection.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsVertical(this FourTypesDirection direction)
        {
            return (direction & (FourTypesDirection.Up | FourTypesDirection.Down)) != FourTypesDirection.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightTypesDirection ToEightTypesDirection(this FourTypesDirection direction)
        {
            return (EightTypesDirection)direction;
        }
    }
}
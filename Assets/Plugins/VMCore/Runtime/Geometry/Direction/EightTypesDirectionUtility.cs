using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class EightTypesDirectionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightTypesDirection SingleFlagReversed(this EightTypesDirection direction)
        {
            return direction switch
            {
                EightTypesDirection.Left => EightTypesDirection.Right,
                EightTypesDirection.Right => EightTypesDirection.Left,
                EightTypesDirection.Up => EightTypesDirection.Down,
                EightTypesDirection.Down => EightTypesDirection.Up,
                EightTypesDirection.UpLeft => EightTypesDirection.DownRight,
                EightTypesDirection.UpRight => EightTypesDirection.DownLeft,
                EightTypesDirection.DownLeft => EightTypesDirection.UpRight,
                EightTypesDirection.DownRight => EightTypesDirection.UpLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightTypesDirection Reversed(this EightTypesDirection direction)
        {
            var result = EightTypesDirection.None;

            if (direction.HasFlag(EightTypesDirection.Left))
            {
                result |= EightTypesDirection.Right;
            }

            if (direction.HasFlag(EightTypesDirection.Right))
            {
                result |= EightTypesDirection.Left;
            }
            
            if (direction.HasFlag(EightTypesDirection.Up))
            {
                result |= EightTypesDirection.Down;
            }

            if (direction.HasFlag(EightTypesDirection.Down))
            {
                result |= EightTypesDirection.Up;
            }

            if (direction.HasFlag(EightTypesDirection.UpLeft))
            {
                result |= EightTypesDirection.DownRight;
            }
            
            if (direction.HasFlag(EightTypesDirection.UpRight))
            {
                result |= EightTypesDirection.DownLeft;
            }
            
            if (direction.HasFlag(EightTypesDirection.DownLeft))
            {
                result |= EightTypesDirection.UpRight;
            }
            
            if (direction.HasFlag(EightTypesDirection.DownRight))
            {
                result |= EightTypesDirection.UpLeft;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int ToCardinalVector(this EightTypesDirection direction)
        {
            return direction switch
            {
                EightTypesDirection.Up => Vector2Int.up,
                EightTypesDirection.Down => Vector2Int.down,
                EightTypesDirection.Left => Vector2Int.left,
                EightTypesDirection.Right => Vector2Int.right,
                EightTypesDirection.UpLeft => CommonVector2Int.upLeft,
                EightTypesDirection.UpRight => CommonVector2Int.upRight,
                EightTypesDirection.DownLeft => CommonVector2Int.downLeft,
                EightTypesDirection.DownRight => CommonVector2Int.downRight,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightTypesDirection GetOtherDirections(this EightTypesDirection direction)
        {
            return EightTypesDirection.All & ~direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHorizontal(this EightTypesDirection direction)
        {
            return (direction & EightTypesDirection.Horizontal) != EightTypesDirection.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsVertical(this EightTypesDirection direction)
        {
            return (direction & EightTypesDirection.Vertical) != EightTypesDirection.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCardinal(this EightTypesDirection direction)
        {
            return (direction & EightTypesDirection.Cardinal) != EightTypesDirection.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCorner(this EightTypesDirection direction)
        {
            return (direction & EightTypesDirection.Corner) != EightTypesDirection.None;
        }
    }
}
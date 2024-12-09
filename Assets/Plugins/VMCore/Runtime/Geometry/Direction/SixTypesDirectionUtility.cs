using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class SixTypesDirectionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SixTypesDirection SingleFlagReversed(this SixTypesDirection direction)
        {
            return direction switch
            {
                SixTypesDirection.Up => SixTypesDirection.Down,
                SixTypesDirection.Down => SixTypesDirection.Up,
                SixTypesDirection.Left => SixTypesDirection.Right,
                SixTypesDirection.Right => SixTypesDirection.Left,
                SixTypesDirection.Forward => SixTypesDirection.Back,
                SixTypesDirection.Back => SixTypesDirection.Forward,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SixTypesDirection Reversed(this SixTypesDirection direction)
        {
            var result = SixTypesDirection.None;

            if (direction.HasFlag(SixTypesDirection.Up))
            {
                result |= SixTypesDirection.Down;
            }

            if (direction.HasFlag(SixTypesDirection.Down))
            {
                result |= SixTypesDirection.Up;
            }

            if (direction.HasFlag(SixTypesDirection.Left))
            {
                result |= SixTypesDirection.Right;
            }

            if (direction.HasFlag(SixTypesDirection.Right))
            {
                result |= SixTypesDirection.Left;
            }

            if (direction.HasFlag(SixTypesDirection.Forward))
            {
                result |= SixTypesDirection.Back;
            }
            
            if (direction.HasFlag(SixTypesDirection.Back))
            {
                result |= SixTypesDirection.Forward;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ToCardinalVector(this SixTypesDirection direction)
        {
            return direction switch
            {
                SixTypesDirection.Up => Vector3Int.up,
                SixTypesDirection.Down => Vector3Int.down,
                SixTypesDirection.Left => Vector3Int.left,
                SixTypesDirection.Right => Vector3Int.right,
                SixTypesDirection.Forward => Vector3Int.forward,
                SixTypesDirection.Back => Vector3Int.back,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SixTypesDirection GetOtherDirections(this SixTypesDirection direction)
        {
            return SixTypesDirection.All & ~direction;
        }
    }
}
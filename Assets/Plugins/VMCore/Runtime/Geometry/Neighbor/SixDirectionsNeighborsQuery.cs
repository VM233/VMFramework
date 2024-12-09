using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class SixDirectionsNeighborsQuery
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this SixDirectionsNeighbors<TItem> allNeighbors,
            SixTypesDirection direction)
        {
            return direction switch
            {
                SixTypesDirection.Left => allNeighbors.left,
                SixTypesDirection.Right => allNeighbors.right,
                SixTypesDirection.Up => allNeighbors.up,
                SixTypesDirection.Down => allNeighbors.down,
                SixTypesDirection.Forward => allNeighbors.forward,
                SixTypesDirection.Back => allNeighbors.back,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this SixDirectionsNeighbors<TItem> allNeighbors,
            FourTypesDirection direction)
        {
            return direction switch
            {
                FourTypesDirection.Left => allNeighbors.left,
                FourTypesDirection.Right => allNeighbors.right,
                FourTypesDirection.Up => allNeighbors.up,
                FourTypesDirection.Down => allNeighbors.down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this SixDirectionsNeighbors<TItem> allNeighbors,
            LeftRightDirection direction)
        {
            return direction switch
            {
                LeftRightDirection.Left => allNeighbors.left,
                LeftRightDirection.Right => allNeighbors.right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this SixDirectionsNeighbors<TItem> allNeighbors, int index)
        {
            return index switch
            {
                0 => allNeighbors.left,
                1 => allNeighbors.right,
                2 => allNeighbors.up,
                3 => allNeighbors.down,
                4 => allNeighbors.forward,
                5 => allNeighbors.back,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TItem> GetNeighbors<TItem>(this SixDirectionsNeighbors<TItem> allNeighbors,
            SixTypesDirection direction)
        {
            if (direction.HasFlag(SixTypesDirection.Left))
            {
                yield return allNeighbors.left;
            }

            if (direction.HasFlag(SixTypesDirection.Right))
            {
                yield return allNeighbors.right;
            }

            if (direction.HasFlag(SixTypesDirection.Up))
            {
                yield return allNeighbors.up;
            }

            if (direction.HasFlag(SixTypesDirection.Down))
            {
                yield return allNeighbors.down;
            }

            if (direction.HasFlag(SixTypesDirection.Forward))
            {
                yield return allNeighbors.forward;
            }

            if (direction.HasFlag(SixTypesDirection.Back))
            {
                yield return allNeighbors.back;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetNeighbors<TItem>(this SixDirectionsNeighbors<TItem> allNeighbors,
            SixTypesDirection direction, ref TItem[] neighbors)
        {
            6.CreateOrResizeArrayWithMinLength(ref neighbors);

            int count = 0;

            if (direction.HasFlag(SixTypesDirection.Left))
            {
                neighbors[count++] = allNeighbors.left;
            }

            if (direction.HasFlag(SixTypesDirection.Right))
            {
                neighbors[count++] = allNeighbors.right;
            }

            if (direction.HasFlag(SixTypesDirection.Up))
            {
                neighbors[count++] = allNeighbors.up;
            }

            if (direction.HasFlag(SixTypesDirection.Down))
            {
                neighbors[count++] = allNeighbors.down;
            }

            if (direction.HasFlag(SixTypesDirection.Forward))
            {
                neighbors[count++] = allNeighbors.forward;
            }

            if (direction.HasFlag(SixTypesDirection.Back))
            {
                neighbors[count++] = allNeighbors.back;
            }

            return count;
        }
    }
}
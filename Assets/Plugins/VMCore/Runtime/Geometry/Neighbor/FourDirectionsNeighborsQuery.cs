using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class FourDirectionsNeighborsQuery
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this FourDirectionsNeighbors<TItem> allNeighbors,
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
        public static TItem GetNeighbor<TItem>(this FourDirectionsNeighbors<TItem> allNeighbors,
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
        public static TItem GetNeighbor<TItem>(this FourDirectionsNeighbors<TItem> allNeighbors, int index)
        {
            return index switch
            {
                0 => allNeighbors.left,
                1 => allNeighbors.right,
                2 => allNeighbors.up,
                3 => allNeighbors.down,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TItem> GetNeighbors<TItem>(this FourDirectionsNeighbors<TItem> allNeighbors,
            FourTypesDirection direction)
        {
            if (direction.HasFlag(FourTypesDirection.Left))
            {
                yield return allNeighbors.left;
            }

            if (direction.HasFlag(FourTypesDirection.Right))
            {
                yield return allNeighbors.right;
            }

            if (direction.HasFlag(FourTypesDirection.Up))
            {
                yield return allNeighbors.up;
            }

            if (direction.HasFlag(FourTypesDirection.Down))
            {
                yield return allNeighbors.down;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetNeighbors<TItem>(this FourDirectionsNeighbors<TItem> allNeighbors,
            FourTypesDirection direction, ref TItem[] neighbors)
        {
            4.CreateOrResizeArrayWithMinLength(ref neighbors);

            int count = 0;

            if (direction.HasFlag(FourTypesDirection.Left))
            {
                neighbors[count++] = allNeighbors.left;
            }

            if (direction.HasFlag(FourTypesDirection.Right))
            {
                neighbors[count++] = allNeighbors.right;
            }

            if (direction.HasFlag(FourTypesDirection.Up))
            {
                neighbors[count++] = allNeighbors.up;
            }

            if (direction.HasFlag(FourTypesDirection.Down))
            {
                neighbors[count++] = allNeighbors.down;
            }

            return count;
        }
    }
}
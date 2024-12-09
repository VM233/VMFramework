using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class LeftRightNeighborsQuery
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this LeftRightNeighbors<TItem> allNeighbors, LeftRightDirection direction)
        {
            return direction switch
            {
                LeftRightDirection.Left => allNeighbors.left,
                LeftRightDirection.Right => allNeighbors.right,
                _ => throw new ArgumentException("Invalid direction", nameof(direction))
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this LeftRightNeighbors<TItem> allNeighbors, int direction)
        {
            return direction switch
            {
                > 0 => allNeighbors.right,
                < 0 => allNeighbors.left,
                _ => throw new ArgumentException("Invalid direction", nameof(direction))
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TItem> GetNeighbors<TItem>(this LeftRightNeighbors<TItem> allNeighbors,
            LeftRightDirection direction)
        {
            if (direction.HasFlag(LeftRightDirection.Left))
            {
                yield return allNeighbors.left;
            }

            if (direction.HasFlag(LeftRightDirection.Right))
            {
                yield return allNeighbors.right;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetNeighbors<TItem>(this LeftRightNeighbors<TItem> allNeighbors, LeftRightDirection direction,
            ref TItem[] neighbors)
        {
            2.CreateOrResizeArrayWithMinLength(ref neighbors);
            
            int count = 0;

            if (direction.HasFlag(LeftRightDirection.Left))
            {
                neighbors[count++] = allNeighbors.left;
            }

            if (direction.HasFlag(LeftRightDirection.Right))
            {
                neighbors[count++] = allNeighbors.right;
            }

            return count;
        }
    }
}
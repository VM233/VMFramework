using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class EightDirectionsNeighborsQuery
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this EightDirectionsNeighbors<TItem> allNeighbors,
            EightTypesDirection direction)
        {
            return direction switch
            {
                EightTypesDirection.UpLeft => allNeighbors.upLeft,
                EightTypesDirection.Up => allNeighbors.up,
                EightTypesDirection.UpRight => allNeighbors.upRight,
                EightTypesDirection.Left => allNeighbors.left,
                EightTypesDirection.Right => allNeighbors.right,
                EightTypesDirection.DownLeft => allNeighbors.downLeft,
                EightTypesDirection.Down => allNeighbors.down,
                EightTypesDirection.DownRight => allNeighbors.downRight,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetNeighbor<TItem>(this EightDirectionsNeighbors<TItem> allNeighbors,
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
        public static TItem GetNeighbor<TItem>(this EightDirectionsNeighbors<TItem> allNeighbors,
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
        public static TItem GetNeighbor<TItem>(this EightDirectionsNeighbors<TItem> allNeighbors, int index)
        {
            return index switch
            {
                0 => allNeighbors.upLeft,
                1 => allNeighbors.up,
                2 => allNeighbors.upRight,
                3 => allNeighbors.left,
                4 => allNeighbors.right,
                5 => allNeighbors.downLeft,
                6 => allNeighbors.down,
                7 => allNeighbors.downRight,
                _ => throw new IndexOutOfRangeException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TItem> GetNeighbors<TItem>(this EightDirectionsNeighbors<TItem> allNeighbors,
            EightTypesDirection direction)
        {
            if (direction.HasFlag(EightTypesDirection.UpLeft))
            {
                yield return allNeighbors.upLeft;
            }

            if (direction.HasFlag(EightTypesDirection.Up))
            {
                yield return allNeighbors.up;
            }
            
            if (direction.HasFlag(EightTypesDirection.UpRight))
            {
                yield return allNeighbors.upRight;
            }
            
            if (direction.HasFlag(EightTypesDirection.Left))
            {
                yield return allNeighbors.left;
            }
            
            if (direction.HasFlag(EightTypesDirection.Right))
            {
                yield return allNeighbors.right;
            }
            
            if (direction.HasFlag(EightTypesDirection.DownLeft))
            {
                yield return allNeighbors.downLeft;
            }
            
            if (direction.HasFlag(EightTypesDirection.Down))
            {
                yield return allNeighbors.down;
            }
            
            if (direction.HasFlag(EightTypesDirection.DownRight))
            {
                yield return allNeighbors.downRight;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetNeighbors<TItem>(this EightDirectionsNeighbors<TItem> allNeighbors,
            EightTypesDirection direction, ref TItem[] neighbors)
        {
            8.CreateOrResizeArrayWithMinLength(ref neighbors);
            
            int count = 0;
            
            if (direction.HasFlag(EightTypesDirection.UpLeft))
            {
                neighbors[count++] = allNeighbors.upLeft;
            }

            if (direction.HasFlag(EightTypesDirection.Up))
            {
                neighbors[count++] = allNeighbors.up;
            }
            
            if (direction.HasFlag(EightTypesDirection.UpRight))
            {
                neighbors[count++] = allNeighbors.upRight;
            }
            
            if (direction.HasFlag(EightTypesDirection.Left))
            {
                neighbors[count++] = allNeighbors.left;
            }
            
            if (direction.HasFlag(EightTypesDirection.Right))
            {
                neighbors[count++] = allNeighbors.right;
            }
            
            if (direction.HasFlag(EightTypesDirection.DownLeft))
            {
                neighbors[count++] = allNeighbors.downLeft;
            }
            
            if (direction.HasFlag(EightTypesDirection.Down))
            {
                neighbors[count++] = allNeighbors.down;
            }
            
            if (direction.HasFlag(EightTypesDirection.DownRight))
            {
                neighbors[count++] = allNeighbors.downRight;
            }
            
            return count;
        }
    }
}
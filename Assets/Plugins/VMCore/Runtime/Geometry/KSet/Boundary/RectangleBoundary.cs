using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public readonly struct RectangleBoundary : IEnumerableKSet<Vector2Int>
    {
        public readonly int minX;
        public readonly int minY;
        public readonly int maxX;
        public readonly int maxY;
        
        public Vector2Int min => new(minX, minY);
        public Vector2Int max => new(maxX, maxY);
        
        public int width => maxX - minX + 1;
        public int height => maxY - minY + 1;

        public int Count
        {
            get
            {
                int width = this.width;
                int height = this.height;
                
                if (width <= 1)
                {
                    if (width <= 0)
                    {
                        return 0;
                    }
                    
                    return height;
                }

                if (height <= 1)
                {
                    if (height <= 0)
                    {
                        return 0;
                    }
                    
                    return width;
                }
                
                var ySide = height - 2;

                return width + width + ySide + ySide;
            }
        }

        public RectangleBoundary(int minX, int minY, int maxX, int maxY)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
        }

        public RectangleBoundary(Vector2Int start, Vector2Int end)
        {
            minX = start.x;
            minY = start.y;
            maxX = end.x;
            maxY = end.y;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Vector2Int pos)
        {
            if (pos.x == minX || pos.x == maxX)
            {
                return pos.y >= minY && pos.y <= maxY;
            }

            if (pos.y == minY || pos.y == maxY)
            {
                return pos.x >= minX && pos.x <= maxX;
            }
            
            return false;
        }

        #region Enumerator

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public struct Enumerator : IEnumerator<Vector2Int>
        {
            private readonly RectangleBoundary boundary;
            private readonly int count;
            private int index;
            private int baseIndex;
            private FourTypesDirection side;

            public Enumerator(RectangleBoundary boundary)
            {
                this.boundary = boundary;
                count = boundary.Count;
                index = -1;
                baseIndex = -1;
                side = FourTypesDirection.Left;
            }

            public Vector2Int Current =>
                side switch
                {
                    FourTypesDirection.Left => new Vector2Int(boundary.minX, boundary.minY + baseIndex),
                    FourTypesDirection.Up => new Vector2Int(boundary.minX + baseIndex, boundary.maxY),
                    FourTypesDirection.Right => new Vector2Int(boundary.maxX, boundary.maxY - baseIndex),
                    FourTypesDirection.Down => new Vector2Int(boundary.maxX - baseIndex, boundary.minY),
                    _ => throw new ArgumentOutOfRangeException()
                };

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                index++;
                baseIndex++;

                switch (side)
                {
                    case FourTypesDirection.Left:
                        if (baseIndex >= boundary.height)
                        {
                            if (boundary.width <= 1)
                            {
                                return false;
                            }

                            baseIndex = 1;
                            side = FourTypesDirection.Up;
                        }

                        break;
                    case FourTypesDirection.Up:
                        if (baseIndex >= boundary.width)
                        {
                            if (boundary.height <= 1)
                            {
                                return false;
                            }

                            baseIndex = 1;
                            side = FourTypesDirection.Right;
                        }

                        break;
                    case FourTypesDirection.Right:
                        if (baseIndex >= boundary.height)
                        {
                            baseIndex = 1;
                            side = FourTypesDirection.Down;
                        }

                        break;
                }

                return index < count;
            }

            public void Reset()
            {
                index = -1;
                baseIndex = -1;
                side = FourTypesDirection.Left;
            }

            public void Dispose()
            {
            }
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct RectangleInteger
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator : IEnumerator<Vector2Int>
        {
            private readonly RectangleInteger rectangle;
            private readonly bool isValid;
            private int x, y;

            public Enumerator(RectangleInteger rectangle)
            {
                this.rectangle = rectangle;
                if (rectangle.min.x > rectangle.max.x || rectangle.min.y > rectangle.max.y)
                {
                    isValid = false;
                    x = y = 0;
                }
                else
                {
                    isValid = true;
                    x = rectangle.min.x;
                    y = rectangle.min.y - 1;
                }
            }

            public Vector2Int Current => new(x, y);

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (isValid == false)
                {
                    return false;
                }
                
                y++;

                if (y > rectangle.max.y)
                {
                    y = rectangle.min.y;
                    x++;

                    if (x > rectangle.max.x)
                    {
                        return false;
                    }
                }
                
                return true;
            }

            public void Reset()
            {
                x = rectangle.min.x;
                y = rectangle.min.y - 1;
            }

            public void Dispose() { }
        }
    }
}
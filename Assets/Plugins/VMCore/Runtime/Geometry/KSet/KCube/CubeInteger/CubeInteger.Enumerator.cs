using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct CubeInteger
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Vector3Int> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator : IEnumerator<Vector3Int>
        {
            private readonly CubeInteger cube;
            private readonly bool isValid;
            private int x, y, z;

            public Enumerator(CubeInteger cube)
            {
                this.cube = cube;
                if (cube.min.x > cube.max.x || cube.min.y > cube.max.y || cube.min.z > cube.max.z)
                {
                    isValid = false;
                    x = y = z = 0;
                }
                else
                {
                    isValid = true;
                    x = cube.min.x;
                    y = cube.min.y;
                    z = cube.min.z - 1;
                }
            }

            public Vector3Int Current => new(x, y, z);

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (isValid == false)
                {
                    return false;
                }
                
                z++;

                if (z > cube.max.z)
                {
                    z = cube.min.z;
                    y++;

                    if (y > cube.max.y)
                    {
                        y = cube.min.y;
                        x++;

                        if (x > cube.max.x)
                        {
                            return false;
                        }
                    }
                }
                
                return true;
            }

            public void Reset()
            {
                x = cube.min.x;
                y = cube.min.y;
                z = cube.min.z - 1;
            }

            public void Dispose() { }
        }
    }
}
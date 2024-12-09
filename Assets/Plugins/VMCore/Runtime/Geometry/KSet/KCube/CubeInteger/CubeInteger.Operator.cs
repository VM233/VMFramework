using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct CubeInteger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out Vector3Int min, out Vector3Int max)
        {
            min = this.min;
            max = this.max;
        }
        
        public static CubeInteger operator +(CubeInteger a, Vector3Int b) =>
            new(a.min + b, a.max + b);

        public static CubeInteger operator -(CubeInteger a, Vector3Int b) =>
            new(a.min - b, a.max - b);

        public static CubeInteger operator *(CubeInteger a, Vector3Int b)
        {
            var xMin = a.min.x;
            var xMax = a.max.x;

            if (b.x >= 0)
            {
                xMin *= b.x;
                xMax *= b.x;
            }
            else
            {
                (xMin, xMax) = (xMax * b.x, xMin * b.x);
            }

            var yMin = a.min.y;
            var yMax = a.max.y;

            if (b.y >= 0)
            {
                yMin *= b.y;
                yMax *= b.y;
            }
            else
            {
                (yMin, yMax) = (yMax * b.y, yMin * b.y);
            }

            var zMin = a.min.z;
            var zMax = a.max.z;

            if (b.z >= 0)
            {
                zMin *= b.z;
                zMax *= b.z;
            }
            else
            {
                (zMin, zMax) = (zMax * b.z, zMin * b.z);
            }

            return new(xMin, yMin, zMin, xMax, yMax, zMax);
        }

        public static CubeInteger operator *(CubeInteger a, int b)
        {
            if (b >= 0)
            {
                return new(a.min * b, a.max * b);
            }

            return new(a.max * b, a.min * b);
        }

        public static CubeInteger operator /(CubeInteger a, Vector3Int b)
        {
            var xMin = a.min.x;
            var xMax = a.max.x;

            if (b.x >= 0)
            {
                xMin /= b.x;
                xMax /= b.x;
            }
            else
            {
                (xMin, xMax) = (xMax / b.x, xMin / b.x);
            }

            var yMin = a.min.y;
            var yMax = a.max.y;

            if (b.y >= 0)
            {
                yMin /= b.y;
                yMax /= b.y;
            }
            else
            {
                (yMin, yMax) = (yMax / b.y, yMin / b.y);
            }

            var zMin = a.min.z;
            var zMax = a.max.z;

            if (b.z >= 0)
            {
                zMin /= b.z;
                zMax /= b.z;
            }
            else
            {
                (zMin, zMax) = (zMax / b.z, zMin / b.z);
            }

            return new(xMin, yMin, zMin, xMax, yMax, zMax);
        }

        public static CubeInteger operator /(CubeInteger a, int b)
        {
            if (b >= 0)
            {
                return new(a.min / b, a.max / b);
            }

            return new(a.max / b, a.min / b);
        }

        public static CubeInteger operator -(CubeInteger a) =>
            new(-a.max, -a.min);

        public static bool operator ==(CubeInteger a, CubeInteger b) =>
            a.Equals(b);

        public static bool operator !=(CubeInteger a, CubeInteger b) =>
            !a.Equals(b);
        
        public static implicit operator (Vector3Int min, Vector3Int max)(CubeInteger cube) => (cube.min, cube.max);
        
        public static implicit operator CubeInteger((Vector3Int min, Vector3Int max) tuple) => new(tuple.min, tuple.max);

        public static implicit operator BoundsInt(CubeInteger cube) =>
            new(cube.min, cube.max - cube.min + Vector3Int.one);
    }
}
using System;
using UnityEngine;

namespace VMFramework.Core
{
    public readonly partial struct CubeFloat : IKCubeFloat<Vector3>, IEquatable<CubeFloat>, IFormattable
    {
        public static CubeFloat Zero { get; } = new(Vector3.zero, Vector3.zero);

        public static CubeFloat One { get; } = new(Vector3.one, Vector3.one);

        public static CubeFloat Unit { get; } = new(Vector3.zero, Vector3.one);

        public Vector3 Size => max - min;

        public Vector3 Pivot => (max + min) / 2;

        public Vector3 Extents => (max - min) / 2;

        public readonly Vector3 min, max;

        public RangeFloat XRange => new(min.x, max.x);

        public RangeFloat YRange => new(min.y, max.y);

        public RangeFloat ZRange => new(min.z, max.z);

        public RectangleFloat XYRectangle => new(min.x, min.y, max.x, max.y);

        public RectangleFloat XZRectangle => new(min.x, min.z, max.x, max.z);

        public RectangleFloat YZRectangle => new(min.y, min.z, max.y, max.z);


        #region Constructor

        public CubeFloat(RangeFloat xRange, RangeFloat yRange, RangeFloat zRange)
        {
            min = new Vector3(xRange.min, yRange.min, zRange.min);
            max = new Vector3(xRange.max, yRange.max, zRange.max);
        }

        public CubeFloat(RectangleFloat xyRectangle, RangeFloat zRange)
        {
            min = new Vector3(xyRectangle.min.x, xyRectangle.min.y, zRange.min);
            max = new Vector3(xyRectangle.max.x, xyRectangle.max.y, zRange.max);
        }

        public CubeFloat(RangeFloat xRange, RectangleFloat yzRectangle)
        {
            min = new Vector3(xRange.min, yzRectangle.min.x, yzRectangle.min.y);
            max = new Vector3(xRange.max, yzRectangle.max.x, yzRectangle.max.y);
        }

        public CubeFloat(float xMin, float yMin, float zMin, float xMax, float yMax, float zMax)
        {
            min = new Vector3(xMin, yMin, zMin);
            max = new Vector3(xMax, yMax, zMax);
        }

        public CubeFloat(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public CubeFloat(float width, float length, float height)
        {
            min = Vector3.zero;
            max = new Vector3(width, length, height);
        }

        public CubeFloat(Vector3 size)
        {
            min = Vector3.zero;
            max = size;
        }

        public CubeFloat(CubeFloat source)
        {
            min = source.min;
            max = source.max;
        }

        public CubeFloat(IMinMaxOwner<Vector3> config)
        {
            if (config is null)
            {
                min = Vector3.zero;
                max = Vector3.zero;
                return;
            }

            min = config.Min;
            max = config.Max;
        }

        #endregion

        #region Equatable

        public bool Equals(CubeFloat other)
        {
            return min.Equals(other.min) && max.Equals(other.max);
        }

        public override bool Equals(object obj)
        {
            return obj is CubeFloat other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(min, max);
        }

        #endregion

        #region To String

        public override string ToString()
        {
            return $"[{min}, {max}]";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"[{min.ToString(format, formatProvider)},{max.ToString(format, formatProvider)}]";
        }

        #endregion
    }
}
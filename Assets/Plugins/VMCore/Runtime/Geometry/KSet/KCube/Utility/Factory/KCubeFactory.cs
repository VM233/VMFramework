using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial struct RangeFloat
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat FromPivotExtents(float pivot, float extents) => new(pivot - extents, pivot + extents);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat FromPivotSize(float pivot, float size)
        {
            float extents = size / 2;
            return new(pivot - extents, pivot + extents);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat FromCorners(float corner1, float corner2) =>
            new(corner1.Min(corner2), corner1.Max(corner2));
    }
    
    public partial struct RangeInteger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger FromPivotExtents(int pivot, int extents) => new(pivot - extents, pivot + extents);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger FromCorners(int corner1, int corner2) =>
            new(corner1.Min(corner2), corner1.Max(corner2));
    }
    
    public partial struct RectangleFloat
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleFloat FromPivotExtents(Vector2 pivot, Vector2 extents) =>
            new(pivot - extents, pivot + extents);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleFloat FromPivotExtents(Vector2 pivot, float extents)
        {
            var extentsVector = new Vector2(extents, extents);
            return new(pivot - extentsVector, pivot + extentsVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleFloat FromPivotSize(Vector2 pivot, Vector2 size)
        {
            var extents = size / 2;
            return new(pivot - extents, pivot + extents);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleFloat FromPivotSize(Vector2 pivot, float size)
        {
            var extents = size / 2;
            var extentsVector = new Vector2(extents, extents);
            return new(pivot - extentsVector, pivot + extentsVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleFloat FromCorners(Vector2 corner1, Vector2 corner2) =>
            new(corner1.Min(corner2), corner1.Max(corner2));
    }
    
    public partial struct RectangleInteger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger FromPivotExtents(Vector2Int pivot, Vector2Int extents) =>
            new(pivot - extents, pivot + extents);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger FromPivotExtents(Vector2Int pivot, int extents)
        {
            var extentsVector = new Vector2Int(extents, extents);
            return new(pivot - extentsVector, pivot + extentsVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger FromCorners(Vector2Int corner1, Vector2Int corner2) =>
            new(corner1.Min(corner2), corner1.Max(corner2));
    }
    
    public partial struct CubeFloat
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeFloat FromPivotExtents(Vector3 pivot, Vector3 extents) =>
            new(pivot - extents, pivot + extents);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeFloat FromPivotExtents(Vector3 pivot, float extents)
        {
            var extentsVector = new Vector3(extents, extents, extents);
            return new(pivot - extentsVector, pivot + extentsVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeFloat FromPivotSize(Vector3 pivot, Vector3 size)
        {
            var extents = size / 2;
            return new(pivot - extents, pivot + extents);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeFloat FromPivotSize(Vector3 pivot, float size)
        {
            var extents = size / 2;
            var extentsVector = new Vector3(extents, extents, extents);
            return new(pivot - extentsVector, pivot + extentsVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeFloat FromCorners(Vector3 corner1, Vector3 corner2) =>
            new(corner1.Min(corner2), corner1.Max(corner2));
    }
    
    public partial struct CubeInteger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger FromPivotExtents(Vector3Int pivot, Vector3Int extents) =>
            new(pivot - extents, pivot + extents);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger FromPivotExtents(Vector3Int pivot, int extents)
        {
            var extentsVector = new Vector3Int(extents, extents, extents);
            return new(pivot - extentsVector, pivot + extentsVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger FromCorners(Vector3Int corner1, Vector3Int corner2) =>
            new(corner1.Min(corner2), corner1.Max(corner2));
    }
    
    public partial struct TesseractFloat
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TesseractFloat FromPivotExtents(Vector4 pivot, Vector4 extents) =>
            new(pivot - extents, pivot + extents);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TesseractFloat FromPivotExtents(Vector4 pivot, float extents)
        {
            var extentsVector = new Vector4(extents, extents);
            return new(pivot - extentsVector, pivot + extentsVector);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TesseractFloat FromPivotSize(Vector4 pivot, Vector4 size)
        {
            var extents = size / 2;
            return new(pivot - extents, pivot + extents);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TesseractFloat FromPivotSize(Vector4 pivot, float size)
        {
            var extents = size / 2;
            var extentsVector = new Vector4(extents, extents);
            return new(pivot - extentsVector, pivot + extentsVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TesseractFloat FromCorners(Vector4 corner1, Vector4 corner2) =>
            new(corner1.Min(corner2), corner1.Max(corner2));
    }

    public partial struct ColorRange
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRange FromPivotExtents(Color pivot, Color extents) =>
            new(pivot - extents, pivot + extents);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRange FromPivotSize(Color pivot, Color size)
        {
            var extents = size / 2;
            return new(pivot - extents, pivot + extents);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRange FromCorners(Color corner1, Color corner2) =>
            new(corner1.Min(corner2), corner1.Max(corner2));
    }
}
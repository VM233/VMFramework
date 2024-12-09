using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class KCubeInsertUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger InsertAsX(this RectangleInteger rectangle, int x)
        {
            var yzMin = rectangle.min;
            var yzMax = rectangle.max;
            return new(x, yzMin.x, yzMin.y, x, yzMax.x, yzMax.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger InsertAsY(this RectangleInteger rectangle, int y)
        {
            var xzMin = rectangle.min;
            var xzMax = rectangle.max;
            return new(xzMin.x, y, xzMin.y, xzMax.x, y, xzMax.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger InsertAsZ(this RectangleInteger rectangle, int z)
        {
            var xyMin = rectangle.min;
            var xyMax = rectangle.max;
            return new(xyMin.x, xyMin.y, z, xyMax.x, xyMax.y, z);
        }
    }
}
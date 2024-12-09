using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class BoxCollider2DUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetRectangle(this BoxCollider2D boxCollider, RectangleFloat rectangle)
        {
            boxCollider.offset = rectangle.Pivot;
            boxCollider.size = rectangle.Size;
        }
    }
}
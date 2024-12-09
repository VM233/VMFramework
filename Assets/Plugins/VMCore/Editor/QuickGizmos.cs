using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core.Editor
{
    public static class QuickGizmos
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawLine(Vector2 start, Vector2 end)
        {
            Gizmos.DrawLine(start.As3DXY(), end.As3DXY());
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawLines<TEnumerable>(Vector2 position, TEnumerable directions)
            where TEnumerable : IEnumerable<Vector2>
        {
            foreach (var ray in directions)
            {
                DrawLine(position, position + ray);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawLines<TEnumerable>(TEnumerable rays) where TEnumerable : IEnumerable<Ray2DInfo>
        {
            foreach (var ray in rays)
            {
                DrawLine(ray.origin, ray.origin + ray.direction * ray.distance);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRectangle(Vector2 origin, RectangleFloat offsetRectangle)
        {
            var rectangle = offsetRectangle + origin;

            var bottomLeft = rectangle.min;
            var topLeft = rectangle.LeftTop;
            var topRight = rectangle.max;
            var bottomRight = rectangle.RightBottom;
            
            DrawLine(bottomLeft, topLeft);
            DrawLine(topLeft, topRight);
            DrawLine(topRight, bottomRight);
            DrawLine(bottomRight, bottomLeft);
        }
    }
}
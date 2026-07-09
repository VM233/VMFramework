using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core.Editor
{
    public static class GizmosUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawLine(Vector3 origin, Vector3 direction, float length)
        {
            Gizmos.DrawLine(origin, origin + direction.normalized * length);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation)
        {
            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, size);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}
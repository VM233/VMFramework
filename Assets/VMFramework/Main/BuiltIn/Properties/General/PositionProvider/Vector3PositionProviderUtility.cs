using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public static class Vector3PositionProviderUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 XY(this IVector3PositionProvider provider)
        {
            return provider.Position.XY();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 XZ(this IVector3PositionProvider provider)
        {
            return provider.Position.XZ();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 YZ(this IVector3PositionProvider provider)
        {
            return provider.Position.YZ();
        }
    }
}
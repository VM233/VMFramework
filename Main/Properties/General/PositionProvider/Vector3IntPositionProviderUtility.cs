using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public static class Vector3IntPositionProviderUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int XY(this IVector3IntPositionProvider provider)
        {
            return provider.Position.XY();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int XZ(this IVector3IntPositionProvider provider)
        {
            return provider.Position.XZ();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int YZ(this IVector3IntPositionProvider provider)
        {
            return provider.Position.YZ();
        }
    }
}
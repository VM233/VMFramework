using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class SignUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(this int num) => System.Math.Sign(num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(this long num) => System.Math.Sign(num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(this float num) => System.Math.Sign(num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(this double num) => System.Math.Sign(num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Sign(this Vector2Int vector) => new(vector.x.Sign(), vector.y.Sign());
        
        public static Vector2Int Sign(this Vector2 vector) => new(vector.x.Sign(), vector.y.Sign());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Sign(this Vector3Int vector) => new(vector.x.Sign(), vector.y.Sign(), vector.z.Sign());
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Sign(this Vector3 vector) => new(vector.x.Sign(), vector.y.Sign(), vector.z.Sign());
    }
}
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class ArrayCreationUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateArray<T>(this Vector3Int size, ref T[,,] array)
        {
            array = new T[size.x, size.y, size.z];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateArray<T>(this Vector2Int size, ref T[,] array)
        {
            array = new T[size.x, size.y];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TryCreateArray<T>(this Vector3Int size, ref T[,,] array)
        {
            array ??= new T[size.x, size.y, size.z];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TryCreateArray<T>(this Vector2Int size, ref T[,] array)
        {
            array ??= new T[size.x, size.y];
        }
    }
}
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class CubeUtility
    {
        /// <summary>
        /// 获取立方体上所有的点，包括内部、边界等等
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger GetCube(this Vector3Int start, Vector3Int end)
        {
            return new CubeInteger(start, end);
        }

        /// <summary>
        /// 获取立方体上所有的点，包括内部、边界等等
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger GetCube(this Vector3Int size)
        {
            return new CubeInteger(Vector3Int.zero, size - Vector3Int.one);
        }
        
        /// <summary>
        /// 获取立方体内部的点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger GetInnerCube(this Vector3Int start, Vector3Int end)
        {
            return new CubeInteger(start + Vector3Int.one, end - Vector3Int.one);
        }

        /// <summary>
        /// 获取立方体内部的点
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CubeInteger GetInnerCube(this Vector3Int size)
        {
            return new CubeInteger(Vector3Int.one, size - new Vector3Int(2, 2, 2));
        }
    }
}
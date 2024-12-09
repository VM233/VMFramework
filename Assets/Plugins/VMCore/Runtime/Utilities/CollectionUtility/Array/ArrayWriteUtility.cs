using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class ArrayWriteUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set<T>(this T[,,] array, Vector3Int pos, T content)
        {
            array[pos.x, pos.y, pos.z] = content;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set<T>(this T[,,] array, T content)
        {
            foreach (var pos in array.GetSize().GetCube())
            {
                array.Set(pos, content);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySet<T>(this T[,,] array, Vector3Int pos, T content) where T : class
        {
            if (array[pos.x, pos.y, pos.z] != null)
            {
                return false;
            }
            
            array[pos.x, pos.y, pos.z] = content;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySet<T>(this T[,,] array, Vector3Int pos, T content, out T existingContent) where T : class
        {
            existingContent = array[pos.x, pos.y, pos.z];
            
            if (existingContent != null)
            {
                return false;
            }
            
            array[pos.x, pos.y, pos.z] = content;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<T>(this T[,,] array, Vector3Int pos, out T item)
            where T : class
        {
            ReferenceUtility.Exchange(ref array[pos.x, pos.y, pos.z], null, out item);
            return item != null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    /// <summary>
    /// Thread-unsafe pool of arrays of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ArrayDefaultPool<T>
    {
        private const int LENGTH_STEP = 5;
        private static readonly Dictionary<int, Stack<T[]>> pools = new();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Get(int length)
        {
            var pool = pools.GetValueOrAddNew(length);

            return pool.Count > 0 ? pool.Pop() : new T[length];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetByMinLength(int minLength)
        {
            var leftLength = minLength % LENGTH_STEP;
            int length;
            if (leftLength == 0)
            {
                length = minLength;
            }
            else
            {
                length = minLength + LENGTH_STEP - leftLength;
            }
            
            return Get(length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return(T[] array)
        {
            var pool = pools.GetValueOrAddNew(array.Length);

            pool.Push(array);
        }
    }
}